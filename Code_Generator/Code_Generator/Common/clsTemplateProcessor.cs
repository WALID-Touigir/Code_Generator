using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Code_Generator.clsDatabaseSchema;

namespace Code_Generator.TemplateProcessor
{
    public class clsTemplateProcessor
    {
        public static ConcurrentDictionary<string, TableSchema> TablesSchema = new ConcurrentDictionary<string, TableSchema>(StringComparer.OrdinalIgnoreCase); //ConcurrentDictionary not Dictionary for Multythreading

        private static class GenerateParts 
        {
            public static (string Fields, string Parameters, string Assignments) GenerateSqlParts(TableSchema table)
            {
                var sbFields = new StringBuilder();           // SQL (Column1, Column2, ...)
                var sbParams = new StringBuilder();            // SQL (@Column1, @Column2, ...)
                var sbAssignments = new StringBuilder();      // SQL (Column1=@Column1, ...)

                bool isFirstNonPk = true;
                foreach (var col in table.Columns.Values.Where(c => !c.IsPrimaryKey))
                {
                    if (!isFirstNonPk)
                    {
                        sbFields.Append(", ");
                        sbParams.Append(", ");
                        sbAssignments.Append(",\r\n\t\t\t\t\t\t\t\t ");
                    }

                    sbFields.Append(col.Name);
                    sbParams.Append("@" + col.Name);
                    sbAssignments.Append($"{col.Name}=@{col.Name}");

                    isFirstNonPk = false;
                }

                return (
                    sbFields.ToString(),
                    sbParams.ToString(),
                    sbAssignments.ToString()
                );
            }



            // -----------------------------------------------------


            public static (string CulomnsTableName,
                            string Properties,
                            string CtorDefaults,
                            string CtorParams,
                            string CtorAssignments,
                            string FindVariablesDeclaration,
                            string FindConstructorOutMapping,
                            string FindConstructorValueMapping,
                            string InsertValueMapping,
                            string UpdateValueMapping) GenerateBusinessLayerParts(TableSchema table)
            {
                var sbProperties = new StringBuilder();              // public type Name { get; set; }
                var culomnName = new StringBuilder();              // For search by culomn
                var sbCtorParams = new StringBuilder();              // Full constructor signature (type var, ...)
                var sbCtorAssignments = new StringBuilder();         // Full constructor body assignments
                var sbCtorDefaults = new StringBuilder();            // Default constructor initializations

                var sbFindVariablesDeclaration = new StringBuilder();  // BL Find method local variable declaration
                var sbFindConstructorOutMapping = new StringBuilder(); // BL method call (out var, ...)
                var sbFindConstructorValueMapping = new StringBuilder(); // BL Constructor call (var, ...)

                var sbInsertValueMapping = new StringBuilder();      // BL Insert method call (this.Prop, ...)
                var sbUpdateValueMapping = new StringBuilder();      // BL Update method call (this.PK, this.Prop, ...)

                bool isFirstCtorParam = true;
                bool isFirstCtorAssignment = true;
                bool isFirstInsertValue = true;
                bool isFirstUpdateValue = true;


                foreach (var col in table.Columns.Values)
                {
                    ReferencedColumnSchema FkSchema = new ReferencedColumnSchema();

                    StringBuilder cmpPrvProperty = new StringBuilder();
                    StringBuilder cmpClassAssignementLazyValue = new StringBuilder();
                    StringBuilder cmpPublicProperty = new StringBuilder();

                    if (table.ReferencedTableFK.ContainsKey(col.Name))
                    {
                        try
                        {   
                            table.ReferencedTableFK.TryGetValue(col.Name, out FkSchema);

                            cmpPrvProperty.Append($"\t\tprivate Lazy<cls{FkSchema.ReferencedTable}>  _{FkSchema.Name}Info;");
                            cmpPublicProperty.Append($"\t\tpublic cls{FkSchema.ReferencedTable} {FkSchema.Name}Info => _{FkSchema.Name}Info.Value;");
                            cmpClassAssignementLazyValue.Append($"new Lazy<cls{FkSchema.ReferencedTable}>(() => cls{FkSchema.ReferencedTable}.Find(this.{FkSchema.Name}))");
                        }
                        catch 
                        {
                            cmpPrvProperty.Clear();
                            cmpClassAssignementLazyValue.Clear();
                            cmpPublicProperty.Clear();
                        }


                    }
                    else
                    {
                        cmpClassAssignementLazyValue.Clear();
                        cmpPublicProperty.Clear();
                        cmpPrvProperty.Clear();
                    }


                    string csharpType = col.CSharpType;
                    string csharpTypeNoNull = col.CSharpType.TrimEnd('?');
                    string paramName;
                    paramName = col.Name.ToLower();


                    culomnName.AppendLine(col.Name + ","); // appending culomn name with comma For search


                    // --- Determine Default Literal ---
                    string defaultValueLiteral = GetDefaultValueLiteral(csharpTypeNoNull);

                    // 1. Properties and Constructors
                    sbProperties.AppendLine($"\t\tpublic {csharpType} {col.Name} {{ get; set; }}");
                    sbCtorDefaults.AppendLine($"\t\t\tthis.{col.Name} = {defaultValueLiteral};");

                    // For combination************************************************************************************************}
                    if (cmpPrvProperty.Length > 0 && cmpPublicProperty.Length > 0)
                    {
                        sbProperties.AppendLine($"{cmpPrvProperty}");
                        sbProperties.AppendLine($"{cmpPublicProperty}");
                    }

                    // Full Constructor Parameters and Assignments
                    if (!isFirstCtorParam) sbCtorParams.Append(", ");
                    sbCtorParams.Append($"{csharpTypeNoNull} {paramName}");
                    isFirstCtorParam = false;







                    if (!isFirstCtorAssignment) sbCtorAssignments.AppendLine();
                    sbCtorAssignments.Append($"\t\t\tthis.{col.Name} = {paramName};");


                    if (cmpClassAssignementLazyValue.Length > 0)//{**********************************************}   For Constructor Assignment
                        sbCtorAssignments.AppendLine($"\n\t\t\t_{col.Name}Info = {cmpClassAssignementLazyValue};");

                    isFirstCtorAssignment = false;


                    // 2. Find Method Mapping
                    if (sbFindConstructorOutMapping.Length > 0) sbFindConstructorOutMapping.Append(", ");
                    sbFindConstructorOutMapping.Append($"out {paramName}");

                    if (sbFindConstructorValueMapping.Length > 0) sbFindConstructorValueMapping.Append(", ");
                    sbFindConstructorValueMapping.Append(paramName);

                    sbFindVariablesDeclaration.AppendLine($"\t\t\t{csharpTypeNoNull} {paramName};");

                    // 3. Save Method Value Mapping

                    // Update Parameters Value Mapping (Includes PK)
                    if (!isFirstUpdateValue) sbUpdateValueMapping.Append(",\r\n");
                    sbUpdateValueMapping.Append($"\t\t\t\tthis.{col.Name}");
                    isFirstUpdateValue = false;

                    // Insert Parameters Value Mapping (Excludes PK)
                    if (!col.IsPrimaryKey)
                    {
                        if (!isFirstInsertValue) sbInsertValueMapping.Append(",\r\n");
                        sbInsertValueMapping.Append($"\t\t\t\tthis.{col.Name}");
                        isFirstInsertValue = false;
                    }
                }

                

                return (
                    culomnName.ToString(0, culomnName.Length - 2),//-2 to remove last comma
                    sbProperties.ToString(),
                    sbCtorDefaults.ToString(),
                    sbCtorParams.ToString(),
                    sbCtorAssignments.ToString(),
                    sbFindVariablesDeclaration.ToString(),
                    sbFindConstructorOutMapping.ToString(),
                    sbFindConstructorValueMapping.ToString(),
                    sbInsertValueMapping.ToString(),
                    sbUpdateValueMapping.ToString()
                );
            }


            // -----------------------------------------------------


            public static (string InsertParams, string UpdateParams, string InsertParameterBuilding, string UpdateParameterBuilding, string FindOutParameters,
                string FindReaderOutAssignments, string FindReaderOutDefaults) GenerateDataLayerParts(TableSchema table, string primaryKeyName, string primaryKeyType)
            {
                var sbInsertParams = new StringBuilder();              // DAL Insert method signature
                var sbUpdateParams = new StringBuilder();              // DAL Update method signature
                var sbInsertParamBuilding = new StringBuilder();       // DAL cmd.Parameters.AddWithValue for INSERT
                var sbUpdateParamBuilding = new StringBuilder();       // DAL cmd.Parameters.AddWithValue for UPDATE

                var sbFindOutParameters = new StringBuilder();         // DAL GetInfo method signature (out type var, ...)
                var sbFindReaderOutAssignments = new StringBuilder();  // DAL SqlDataReader assignment block
                var sbFindReaderOutDefaults = new StringBuilder();     // DAL assignment to default value

                bool isFirstNonPk = true;
                bool isFirstOutParam = true;

                // 1. Handle Primary Key for Update Parameters first
                sbUpdateParams.Append($"\t\t\t{primaryKeyType} {primaryKeyName},");

                // 2. Loop through all columns
                foreach (var col in table.Columns.Values)
                {
                    string csharpType = col.CSharpType;
                    string csharpTypeNoNull = col.CSharpType.TrimEnd('?');
                    string paramName = col.Name.ToLower();

                    // --- Determine Default Literal ---
                    string defaultValueLiteral = GetDefaultValueLiteral(csharpTypeNoNull);

                    // 3. Insert/Update Method Signatures & Parameter Building
                    if (!col.IsPrimaryKey)
                    {
                        if (!isFirstNonPk)
                        {
                            sbInsertParams.Append(",\r\n");
                            sbUpdateParams.Append(",\r\n");
                        }

                        // Method Signature Parameters (Insert/Update)
                        sbInsertParams.Append($"\t\t\t{csharpType} {col.Name}");
                        sbUpdateParams.Append($"\t\t\t{csharpType} {col.Name}");

                        // Parameter Building
                        string paramBuilding = GetParameterBuildingCode(col.Name, csharpType, csharpTypeNoNull);
                        sbInsertParamBuilding.AppendLine(paramBuilding);
                        sbUpdateParamBuilding.AppendLine(paramBuilding);

                        isFirstNonPk = false;
                    }
                    else // Primary Key
                    {
                        // Add PK to Update Parameter Building block
                        sbUpdateParamBuilding.AppendLine(GetParameterBuildingCode(col.Name, csharpType, csharpTypeNoNull));
                    }

                    // 4. Find Method OUT Logic (Reader Assignments & Defaults)

                    // DAL Find Method OUT Parameter List (Signature: out type var)
                    if (!isFirstOutParam) sbFindOutParameters.Append(",\r\n");
                    sbFindOutParameters.Append($"\t\t\tout {csharpTypeNoNull} {paramName}");
                    isFirstOutParam = false;

                    // DAL Find Method Reader ASSIGNMENTS
                    if (col.IsNullable || csharpTypeNoNull == "string")
                    {
                        // Use ternary operator for concise assignment (handles DBNull.Value)
                        sbFindReaderOutAssignments.AppendLine($"\t\t\t\t\t{paramName} = (reader[\"{col.Name}\"] != DBNull.Value) ? ({csharpTypeNoNull})reader[\"{col.Name}\"] : {defaultValueLiteral};");
                    }
                    else
                    {
                        // Simple cast for non-nullable value types
                        sbFindReaderOutAssignments.AppendLine($"\t\t\t\t\t{paramName} = ({csharpTypeNoNull})reader[\"{col.Name}\"];");
                    }

                    // DAL Find Method Reader DEFAULTS
                    sbFindReaderOutDefaults.AppendLine($"\t\t\t\t{paramName} = {defaultValueLiteral};");
                }

                return (
                    sbInsertParams.ToString(),
                    sbUpdateParams.ToString(),
                    sbInsertParamBuilding.ToString(),
                    sbUpdateParamBuilding.ToString(),
                    sbFindOutParameters.ToString(),
                    sbFindReaderOutAssignments.ToString(),
                    sbFindReaderOutDefaults.ToString()
                );
            }


            public static class DTO
            {

                public static (
                                string cmpClassLazyValueDeclarations,
                                string ConstructorDefaults,
                                string cmpClassAssignementLazyValue,
                                string CulomnsTableName
                                ) GenerateBusinessLayerDTOParts(TableSchema table)
                {
                    var cmpClassLazyValueDeclarations = new StringBuilder();
                    var ConstructorDefaults = new StringBuilder();
                    var cmpClassAssignementLazyValue = new StringBuilder();
                    var CulomnsTableName = new StringBuilder();

                    foreach (var col in table.Columns.Values)
                    {
                        ReferencedColumnSchema FkSchema = new ReferencedColumnSchema();

                        // If this column is a foreign key — generate Lazy composition
                        if (table.ReferencedTableFK.ContainsKey(col.Name))
                        {
                            try
                            {
                                table.ReferencedTableFK.TryGetValue(col.Name, out FkSchema);

                                // Declare Lazy field and public property
                                cmpClassLazyValueDeclarations.AppendLine(
                                    $"\t\tprivate Lazy<cls{FkSchema.ReferencedTable}> _{FkSchema.Name}Info;");
                                cmpClassLazyValueDeclarations.AppendLine(
                                    $"\t\tpublic cls{FkSchema.ReferencedTable} {FkSchema.Name}Info => _{FkSchema.Name}Info.Value;");

                                // Assign Lazy value in constructor
                                cmpClassAssignementLazyValue.AppendLine(
                                    $"\t\t\t_{FkSchema.Name}Info = new Lazy<cls{FkSchema.ReferencedTable}>(() => " +
                                    $"Info.{FkSchema.Name} > 0 ? cls{FkSchema.ReferencedTable}.Find(Info.{FkSchema.Name}) : null);");
                            }
                            catch
                            {
                                // ignore broken foreign key mapping
                            }
                        }

                        // Default constructor initializations
                        string defaultValueLiteral = GetDefaultValueLiteral(col.CSharpType.TrimEnd('?'));
                        ConstructorDefaults.AppendLine($"\t\t\t{col.Name} = {defaultValueLiteral},");

                        // Build comma-separated column names
                        CulomnsTableName.Append(col.Name + ", ");
                    }

                    // Clean up trailing commas
                    string ctorDefaults = ConstructorDefaults.ToString().TrimEnd(',', '\r', '\n', ' ');
                    string columnNames = CulomnsTableName.ToString().TrimEnd(',', ' ');

                    // Return in the same declared order
                    return (
                        cmpClassLazyValueDeclarations.ToString(),
                        ctorDefaults,
                        cmpClassAssignementLazyValue.ToString(),
                        columnNames
                    );
                }



                // -----------------------------------------------------


                public static (
                        string FindReaderAssignments,
                        string FindReaderOutDefaults,
                        string InsertParameterBuilding,
                        string UpdateParameterBuilding) 
                GenerateDataLayerDTOParts(TableSchema table, string primaryKeyName, string primaryKeyType)
                {
                    var sbFindReaderOutAssignments = new StringBuilder();
                    var sbFindReaderOutDefaults = new StringBuilder();
                    var sbInsertParameterBuilding = new StringBuilder();
                    var sbUpdateParameterBuilding = new StringBuilder();

                    foreach (var col in table.Columns.Values)
                    {
                        string csharpType = col.CSharpType;
                        string csharpTypeNoNull = col.CSharpType.TrimEnd('?');
                        string paramName = col.Name.ToLower();

                        // Determine default literal for the type (e.g. "" for string, 0 for int)
                        string defaultValueLiteral = GetDefaultValueLiteral(csharpTypeNoNull);

                        // ===============================
                        // 1️⃣  Find Reader Assignments (for DTO reading)
                        // ===============================
                        if (col.IsNullable || csharpTypeNoNull == "string")
                        {
                            sbFindReaderOutAssignments.AppendLine(
                                $"\t\t\t\t{col.Name} = (reader[\"{col.Name}\"] != DBNull.Value) ? ({csharpTypeNoNull})reader[\"{col.Name}\"] : {defaultValueLiteral},");
                        }
                        else
                        {
                            sbFindReaderOutAssignments.AppendLine(
                                $"\t\t\t\t{col.Name} = ({csharpTypeNoNull})reader[\"{col.Name}\"],");
                        }

                        // ===============================
                        // 2️⃣  Default Reader Value (for initializing before reading)
                        // ===============================
                        sbFindReaderOutDefaults.AppendLine($"\t\t\t{col.Name.ToLower()} = {defaultValueLiteral};");

                        // ===============================
                        // 3️⃣  Insert Parameters
                        // ===============================
                        if (!col.IsPrimaryKey)
                        {
                            sbInsertParameterBuilding.AppendLine(
                                $"\t\t\t\tcommand.Parameters.AddWithValue(\"@{col.Name}\", Info.{col.Name} ?? (object)DBNull.Value);");
                        }

                        // ===============================
                        // 4️⃣  Update Parameters (includes PK)
                        // ===============================
                        sbUpdateParameterBuilding.AppendLine(
                            $"\t\t\t\tcommand.Parameters.AddWithValue(\"@{col.Name}\", Info.{col.Name} ?? (object)DBNull.Value);");
                    }

                    // ===============================
                    // Return placeholders for template replacement
                    // ===============================
                    return (
                        sbFindReaderOutAssignments.ToString().TrimEnd(',', '\r', '\n'),
                        sbFindReaderOutDefaults.ToString().Trim(),
                        sbInsertParameterBuilding.ToString().Trim(),
                        sbUpdateParameterBuilding.ToString().Trim()
                    );
                }




            }

        }

        // -----------------------------------------------------
        // -----------------------------------------------------
        // --- Helper Functions ---

        static string ReplacePlaceholders(string template, Dictionary<string, string> replacements)
        {
            foreach (var kvp in replacements)
            {
                template = template.Replace(kvp.Key, kvp.Value);
            }
            return template;
        }


        private static string GetDefaultValueLiteral(string csharpTypeNoNull)
        {
            if (csharpTypeNoNull == "string") return "\"\"";
            if (csharpTypeNoNull == "bool") return "false";
            if (csharpTypeNoNull == "DateTime") return "DateTime.MinValue";
            if (csharpTypeNoNull == "decimal" || csharpTypeNoNull == "float" || csharpTypeNoNull == "double") return "0";

            // Default for int, short, long, byte, etc.
            return "-1";
        }

        private static string GetParameterBuildingCode(string columnName, string csharpType, string csharpTypeNoNull)
        {
            // Note: The original logic for string checked against "" and null, but not DBNull on read for non-nullable types.
            // The provided logic is maintained for consistency with the original function's intent.

            if (csharpTypeNoNull == "string")
            {
                return $@"
        if ({columnName} != null && {columnName} != """")
            command.Parameters.AddWithValue(""@{columnName}"", {columnName});
        else
            command.Parameters.AddWithValue(""@{columnName}"", System.DBNull.Value);";
            }
            else if (csharpType.Contains("?"))
            {
                // Check for null for nullable value types
                return $@"
        if ({columnName}.HasValue)
            command.Parameters.AddWithValue(""@{columnName}"", {columnName}.Value);
        else
            command.Parameters.AddWithValue(""@{columnName}"", System.DBNull.Value);";
            }
            else
            {
                // Simple assignment for non-nullable value types
                return $"\t\t\tcommand.Parameters.AddWithValue(\"@{columnName}\", {columnName});";
            }
        }

        //______________________________________________________


        public static string BuildPlaceholders(string templateText, TableSchema table, string projectNamePrefix)
        {

            var replacements = new Dictionary<string, string>();

            // Get Primary Key Info
            string primaryKeyName = table.PrimaryKey?.Name ?? "ID";
            string primaryKeyType = table.PrimaryKey?.CSharpType.TrimEnd('?') ?? "int";

            // 1. Generate core SQL DTO parts
            var sqlParts = GenerateParts.GenerateSqlParts(table);

            if (clsGlobal.AddingDTOclass)
            {


                // 2. Generate Business Logic (BL) DTO parts
                var blParts = GenerateParts.DTO.GenerateBusinessLayerDTOParts(table);

                // 3. Generate Data Access Layer (DAL) DTO parts
                var dalParts = GenerateParts.DTO.GenerateDataLayerDTOParts(table, primaryKeyName, primaryKeyType);

                // 4. Combine all results into a dictionary
                replacements = new Dictionary<string, string>
                {
                    // === PROJECT PREFIX (NEW) ===
                    { "{ProjectPrefix}", projectNamePrefix },
        
                    // === Core Table/Key Info ===
                    { "{TableName}", table.TableName },
                    { "{PrimaryKey}", primaryKeyName },
                    { "{PrimaryKeyType}", primaryKeyType },

                    // === BL PLACEHOLDERS ===
                    { "{cmpClassLazyValueDeclarations}", blParts.cmpClassLazyValueDeclarations },
                    { "{ConstructorDefaults}", blParts.ConstructorDefaults.Trim() },
                    { "{cmpClassAssignementLazyValue}", blParts.cmpClassAssignementLazyValue },
                    { "{CulomnsTableName}", blParts.CulomnsTableName },


                    // === DAL PLACEHOLDERS (Methods) ===

                    { "{FindReaderAssignments}", dalParts.FindReaderAssignments.ToString().Trim() },
                    { "{FindReaderOutDefaults}", dalParts.FindReaderOutDefaults.ToString().Trim() },
                    { "{InsertParameterBuilding}", dalParts.InsertParameterBuilding.ToString().Trim() },
                    { "{UpdateParameterBuilding}", dalParts.UpdateParameterBuilding.ToString().Trim() },
        
                    // === DAL PLACEHOLDERS (SQL) ===
                    { "{FieldList}", sqlParts.Fields.ToString() },
                    { "{ParameterList}", sqlParts.Parameters.ToString() },
                    { "{UpdateSetList}", sqlParts.Assignments.ToString() },
        
                    // Other template placeholders
                    { "{UniqueFieldMethods}", "" } // not implemented yet
                };

            }
            else
            {

                // 2. Generate Business Logic (BL) parts
                var blParts = GenerateParts.GenerateBusinessLayerParts(table);

                // 3. Generate Data Access Layer (DAL) parts
                var dalParts = GenerateParts.GenerateDataLayerParts(table, primaryKeyName, primaryKeyType);

                // 4. Combine all results into a dictionary
                replacements = new Dictionary<string, string>
                {
                // === PROJECT PREFIX (NEW) ===
                { "{ProjectPrefix}", projectNamePrefix },
        
                // === Core Table/Key Info ===
                { "{TableName}", table.TableName },
                { "{PrimaryKey}", primaryKeyName },
                { "{PrimaryKeyType}", primaryKeyType },

                // === BL PLACEHOLDERS ===
                { "{PropertiesPlaceholders}", blParts.Properties.ToString() },
                { "{CulomnsTableName}", blParts.CulomnsTableName },
                { "{ConstructorDefaults}", blParts.CtorDefaults.ToString().Trim() },
                { "{ConstructorParameters}", blParts.CtorParams.ToString() },
                { "{ConstructorAssignments}", blParts.CtorAssignments.ToString().Trim() },

                { "{FindVariablesDeclaration}", blParts.FindVariablesDeclaration.ToString().Trim() },
                { "{FindConstructorOutMapping}", blParts.FindConstructorOutMapping.ToString().Trim() },
                { "{FindConstructorValueMapping}", blParts.FindConstructorValueMapping.ToString().Trim() }, 
        
                // Mappings for BL Save methods
                { "{InsertParametersValueMapping}", blParts.InsertValueMapping.ToString().Trim() },
                { "{UpdateParametersValueMapping}", blParts.UpdateValueMapping.ToString().Trim() },

                // === DAL PLACEHOLDERS (Methods) ===
                { "{FindOutParameters}", dalParts.FindOutParameters.ToString().Trim() },
                { "{FindReaderOutAssignments}", dalParts.FindReaderOutAssignments.ToString().Trim() },
                { "{FindReaderOutDefaults}", dalParts.FindReaderOutDefaults.ToString().Trim() },

                { "{InsertParameters}", dalParts.InsertParams.ToString().Trim() },
                { "{InsertParameterBuilding}", dalParts.InsertParameterBuilding.ToString().Trim() },

                { "{UpdateParameters}", dalParts.UpdateParams.ToString().Trim() },
                { "{UpdateParameterBuilding}", dalParts.UpdateParameterBuilding.ToString().Trim() },
        
                // === DAL PLACEHOLDERS (SQL) ===
                { "{FieldList}", sqlParts.Fields.ToString() },
                { "{ParameterList}", sqlParts.Parameters.ToString() },
                { "{UpdateSetList}", sqlParts.Assignments.ToString() },
        
                // Other template placeholders
                { "{UniqueFieldMethods}", "" } // not implemented yet
            };

            }


            return ReplacePlaceholders(templateText, replacements);
        }





        static string BuildPlaceholdersForDTO(string templateText, TableSchema table, string projectName)
        {
            StringBuilder sbProperties = new StringBuilder();
            StringBuilder sbPublicProperty = new StringBuilder();

            ReferencedColumnSchema FkSchema = new ReferencedColumnSchema();


            foreach (var col in table.Columns.Values)
            {
                if (table.ReferencedTableFK.ContainsKey(col.Name))
                {
                    table.ReferencedTableFK.TryGetValue(col.Name, out FkSchema);

                    try
                    {
                        table.ReferencedTableFK.TryGetValue(col.Name, out FkSchema);


                        sbPublicProperty.AppendLine($"\t\tpublic cls{FkSchema.ReferencedTable}DTO {FkSchema.Name}Info  {{ get; set; }}");
                    }
                    catch { }
                }

                sbProperties.AppendLine($"\t\tpublic {col.CSharpType} {col.Name} {{ get; set; }}");



            }


            var replacements = new Dictionary<string, string>
            {
                {"{CompositionRelationShips}",sbPublicProperty.ToString() },
                { "{PropertiesPlaceholders}",sbProperties.ToString()}, 
                {"{TableName}",table.TableName },
                {"{ProjectPrefix}",projectName }

            };

            return ReplacePlaceholders(templateText, replacements);
        }




        static string ReadTemplate(string templatePath)
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template not found: {templatePath}");

            return File.ReadAllText(templatePath);
        }

        private static string BuildPlaceholders(string template, string projectName, string connectionString)
        {
            if (template == null) return"";

            template = template.Replace("{ProjectPrefix}", projectName);
            template = template.Replace("{ConnectionString}", connectionString);

            return template;
        }








        public static bool GenerateAllLayers()
        {

            try
            {
                if (TablesSchema == null || TablesSchema.Count == 0)
                    return true;

                string templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Templates");
                
                Task.Run(() =>
                {
                    // Data Access Settings
                    string dasTemplate = ReadTemplate(Path.Combine(templatesPath, "DAS_Template.txt"));
                    string dasCode = BuildPlaceholders(dasTemplate, clsGlobal.ProjectName, clsConnectionManager.clsDBconnetionstring.GetAppConnectionString(clsGlobal.DataBaseName));
                    clsUtils.GenerateFile(Path.Combine(clsGlobal.PathFilesToGenerate, clsGlobal.ProjectName, "Data Access Layer"), @"clsDataAccessSettings.cs", dasCode);


                });


                Dictionary<string, string> CachedTemplates;

                if (clsGlobal.AddingDTOclass)
                {
                    // Load once For DTO 
                    CachedTemplates = new Dictionary<string, string>()
                    {
                        { "DAL", File.ReadAllText(Path.Combine(templatesPath,"DTO_DAL_Template.txt")) },
                        { "BL", File.ReadAllText(Path.Combine(templatesPath,"DTO_BL_Template.txt")) },
                        { "DTO", File.ReadAllText(Path.Combine(templatesPath,"DTO_Template.txt")) }
                    };

                }
                else
                {
                    // Load once
                    CachedTemplates = new Dictionary<string, string>()
                    {
                        { "DAL", File.ReadAllText(Path.Combine(templatesPath,"DAL_Template.txt") )},
                        { "BL", File.ReadAllText(Path.Combine(templatesPath,"BL_Template.txt")) },
                    };
                }

                try
                {

                    Parallel.ForEach(TablesSchema.Values, table =>
                    {
                        string dalTemplate = CachedTemplates["DAL"];
                        string dalCode = BuildPlaceholders(dalTemplate, table, clsGlobal.ProjectName);
                        clsUtils.GenerateFile(Path.Combine(clsGlobal.PathFilesToGenerate, clsGlobal.ProjectName, "Data Access Layer"), $"cls{table.TableName}Data.cs", dalCode);


                        string blTemplate = CachedTemplates["BL"];
                        string blCode = BuildPlaceholders(blTemplate, table, clsGlobal.ProjectName);
                        clsUtils.GenerateFile(Path.Combine(clsGlobal.PathFilesToGenerate, clsGlobal.ProjectName, "Business Layer"), $"cls{table.TableName}.cs", blCode);

                        if (clsGlobal.AddingDTOclass)
                        {
                            string dtoTemplate = CachedTemplates["DTO"];
                            string dtoCode = BuildPlaceholdersForDTO(dtoTemplate, table, clsGlobal.ProjectName);
                            clsUtils.GenerateFile(Path.Combine(clsGlobal.PathFilesToGenerate, clsGlobal.ProjectName, "DTO Classes"), $"cls{table.TableName}DTO.cs", dtoCode);

                        }


                    });

                }
                catch{ // log later
                       }

                return true;
            }
            catch { return false; }

        }

    }
}
