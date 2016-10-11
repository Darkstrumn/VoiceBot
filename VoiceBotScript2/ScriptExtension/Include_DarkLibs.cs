using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;//<<--needed for Dictionary
using System.Data; //<<--in references
using System.Data.OleDb; //<<--provided by System.Data.DataSetExtensions.dll
using System.Diagnostics;//<<--needed for debug (visual studio IDE only)
using System.Drawing; //<<--in references
using System.IO;//<<--text file io for loading includes files
using System.Linq;//<<--provided by C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1\\Profile\\Client\\System.Data.Linq.dll
using System.Management; //<<--in references
using System.Reflection;
using System.Speech.Recognition;//<<-- provided by C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1\\Profile\\Client\\System.Speech.dll
using System.Speech.Synthesis;
using System.Text; //<<--provided by mscorlib.dll
using System.Threading;
using System.Threading.Tasks;
using System.Web; //<<--in references
using System.Windows; //<<--in references
using System.Xml; //<<--in references
using Microsoft.CSharp; //<<--in system.dll
using BFS;

//=============================================================================
/// <summary>
/// Includes
/// How it works: includes are accomplished using the ScriptEngine coded above to
/// in-line compile the "include" code into an assembly in memory. The in-memory
/// assembly is then accessed directly or via delegates that connect the includes code to the
/// Includes namespace.
/// calls can then be made fully qualifying the call ie:
/// string str_confirmation_response = Includes.Speech.SpeechRecognizer("Are your sure?");
/// or directly:
/// Assembly assem_Speech = (Assembly)Includes.SupportClasses.ScriptEngine.LoadInclude(new IntPtr(),"Include_DarkLibs",Includes.SupportClasses.Constants.str_default_references)).GetTypes().Where( x => x.FullName.Contains("Includes.Speech");
/// string str_confirmation_response = (string)CallFunction(assem_Speech, "SpeechRecognizer", new object[] {"Are your sure?"})
/// -----------------------------------------------------------------------
/// Usings: using System.IO;//<<--text file io for loading includes files
/// References: N/A
/// NOTE: Includes can be done in 2 ways, using a separate .cs file to hold the
/// cs code loaded via LoadIncludes(), and\or the VoiceBot registry variables loaded
/// via GetIncludes(). The latter is more portable, easier to edit and backup,
/// however, the former, is more compact and self-contained being stored in the
/// registry. The choice is yours on which to use.
/// </summary>
namespace Includes
    {
    //=========================================================================

    #region Support Classes

    //=========================================================================
    //== Support Classes
    //=========================================================================
    namespace SupportClasses
        {
        #region Constants

        //=====================================================================
        public static class Constants
            {
            //-----------------------------------------------------------------
            public static string str_default_references = "System.dll | System.Core.dll |System.Data.dll | System.Drawing.dll | System.Management.dll | System.Web.dll | System.Windows.Forms.dll | System.Xml.dll | mscorlib.dll |  C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\Microsoft.CSharp.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Speech.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Data.Linq.dll".Replace("\\", "\\\\");
            public static string str_default_connection_string = "Provider='Microsoft.ACE.OLEDB.12.0'; Data Source='{PATH}LogBook.mdb'; Persist Security Info=False".Replace("{PATH}", ( BFS.General.GetAppInstallPath() + "\\ScriptExtension\\" )).Replace("\\", "\\\\");
            /*
            The following path will likely need to be created to store any includes you make, as a common place to put them.
            The dir can be created using the following cmd from the command prompt:
            mkdir %LOCALAPPDATA%\VoiceBot\ScriptExtension\
            The path created should be something similar the following:
            C:\Users\YOUR_USER_NAME_HERE\AppData\Local\VoiceBot\ScriptExtension\
            */
            public static string str_default_Include_path = ( BFS.General.GetAppInstallPath() != "C:\\mnt\\W\\DevCore\\ProtoLab\\dotnet\\dotnet2k15\\Projects\\VoiceBotScript\\VoiceBotScript\\bin\\Debug" ) ?
            ( Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\VoiceBot\\ScriptExtension\\" ).Replace("\\", "\\\\")
            : "C:\\mnt\\DevCore\\ProtoLab\\dotnet\\dotnet2k15\\Projects\\VoiceBotScript\\VoiceBotScript\\ScriptExtension\\".Replace("\\", "\\\\");
            //-----------------------------------------------------------------
            }/*</class Constants>*/

        #endregion Constants

        //=====================================================================

        #region ScriptEngine

        //=====================================================================
        //== ScriptEngine Function Library
        //=====================================================================
        /// <summary>
        /// Author: Darkstrumn:\created::160115.03
        /// Function: define standard library functions - The Toolbox -
        /// with the GetInclude functionality, additional functions can be stored as variables,
        /// and loaded dynamically for common library functionaity
        /// </summary>
        public static class ScriptEngine
            {
            //-------------------------------------------------------------------
            public static object Eval(IntPtr windowHandle, string str_code_block_name, string str_source_cs, string str_references = "")
                {
                StringBuilder sb_script_core = new StringBuilder("");
                object[] obj_parameters_array;
                string str_class_name = "TriggerLogic";
                string str_function_name = "{CLASS}.Run".Replace("{CLASS}", str_class_name);
                string str_code;
                string str_code_template = @"
//=============================================================================
//=={CODEBLOCKNAME}
//=============================================================================
using System;
using System.Drawing;
namespace Includes
{
    public class TriggerLogic
    {
        private bool _bln_result = true;
        private string _str_error = ""ok"";
        public TriggerLogic()
            {;}
        public bool Result{get{return(_bln_result);}}
        public string LastError{get{return(_str_error);}}
	    public bool Run(IntPtr windowHandle)
	    {
        try
            {
		    {CODEBLOCK}
            }
        catch(Exception error)
            {
            _str_error = ""TriggerLogic: fail - {ERROR}."".Replace(""{ERROR}"", error.Message);
            }

        return(Result);
	    }
    }
}";
                /*
                execute VHP's (variable hardpoints), allows us to add token handling to the code builder, can be a one-liner,
                but is clearer broken down. Note: order is important.
                */
                str_code = str_code_template.Replace("{CODEBLOCK}", str_source_cs);
                str_code = str_code.Replace("{CODEBLOCKNAME}", str_code_block_name);
                str_code = str_code.Replace("{REFERENCES}", Includes.SupportClasses.Constants.str_default_references);
                sb_script_core.Append(str_code);//</sb_script_core>
                Assembly assembly_trigger_logic = (Assembly)ScriptEngine.CsCodeAssembler(windowHandle, str_code_block_name, sb_script_core.ToString(), str_references = "");
                obj_parameters_array = new object[] { };
                dynamic obj_class = CreateClassInstance(assembly_trigger_logic, str_class_name, obj_parameters_array);
                object obj_return = obj_class.Run(windowHandle);
                //object obj_return = CallFunction(assembly_trigger_logic, str_function_name, obj_parameters_array);
                return ( obj_return );
                }//</Eval( IntPtr windowHandle, string str_code_block_name, string str_source_cs, string str_references = "")>
            //-------------------------------------------------------------------
            /// <summary>
            /// Author: Darkstrumn
            /// Function: CsCodeAssembler takes provided CS source and attempts to compile code, then returns an instance object of the assembly
            /// viable for loading aux scripts to form includes or dynamic code loading
            /// </summary>
            /// <param name="windowHandle"></param>
            /// <param name="str_source_cs"></param>
            /// <param name="str_references"></param>
            /// <returns></returns>
            public static object CsCodeAssembler(IntPtr windowHandle, string str_code_block_name, string str_source_cs, string str_references = "")
                {
                object obj_return = null;
                str_references = ( str_references.Length != 0 ? str_references : Includes.SupportClasses.Constants.str_default_references );
                //
                //CodeDomProvider provider = new CSharpCodeProvider(new Dictionary<String, String>{{ "CompilerVersion","v4.0" }});//<<-- needes: using Microsoft.CSharp;
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                //
                CompilerParameters cp_compiler_params = new CompilerParameters();
                //>>>>>compileing options to build code
                cp_compiler_params.CompilerOptions = "/t:library";//<<--craft as library dll
                cp_compiler_params.GenerateExecutable = false;//<<--do not craft executable
                cp_compiler_params.GenerateInMemory = true;//<<--output in memmory vs output file
                cp_compiler_params.TreatWarningsAsErrors = false;//<<--warning handling
                str_references += ( str_references.Length > 0 ? "|" : "" ) + "C:\\Program Files (x86)\\VoiceBot\\VoiceBot.exe";
                foreach ( string str_reference in str_references.Split('|') )
                    {
                    cp_compiler_params.ReferencedAssemblies.Add(str_reference.Trim());
                    }//</foreach(string str_reference in str_references.Split('|'))>
                     //
                StringBuilder sb_script_core = new StringBuilder("");//<<--more flexible than string concatination if we add hardcoded library logic here
                                                                     //>>>>>ScriptCore.Includes.INCLUDE_CLASS.INCLUDE_FUNCTION
                sb_script_core.Append(@"//<reserved for future use>namespace Library
            //{
            " + str_source_cs + @"
            //}/*</namespace::Library>*/
            ");//</sb_script_core>
               //>>>>>attempt to compile
                CompilerResults cr_result = provider.CompileAssemblyFromSource(cp_compiler_params, sb_script_core.ToString());
                //
                if ( cr_result.Errors.Count > 0 )//>>>>>report error, and fail
                    {
                    foreach ( CompilerError CompErr in cr_result.Errors )
                        {
                        string str_line = sb_script_core.ToString().Split('\n')[CompErr.Line - 1];
                        //BFS.Dialog.ShowMessageError("Source:\n{SOURCE}".Replace("{SOURCE}",sb_script_core.ToString()) );
                        BFS.Dialog.ShowMessageError("CsCodeAssembler\\\\ERROR: Source {NAME}".Replace("{NAME}", str_code_block_name));
                        BFS.Dialog.ShowMessageError("CsCodeAssembler\\\\ERROR: Line number {LINE}, Error Number: {NUMBER}, '{TEXT}'".Replace("{LINE}", CompErr.Line.ToString()).Replace("{NUMBER}", CompErr.ErrorNumber).Replace("{TEXT}", CompErr.ErrorText));
                        BFS.Dialog.ShowMessageError("CsCodeAssembler\\\\ERROR: Code on line number {NUMBER} => '{LINE}'".Replace("{NUMBER}", CompErr.Line.ToString()).Replace("{LINE}", str_line));
                        }//</foreach(CompilerError CompErr in cr_result.Errors)>
                    }
                else//>>>>>execute code
                    {
                    System.Reflection.Assembly assembly_library_code = cr_result.CompiledAssembly;
                    //<moved>object obj_library_code_instance = assembly_library_code.CreateInstance("ScriptCore.Includes");
                    //obj_return = obj_library_code_instance;
                    obj_return = assembly_library_code;
                    }
                //</if(cr_result.Errors.Count > 0)>
                return ( obj_return );
                }/*</CsCodeAssembler( IntPtr windowHandle, string str_source_cs, string str_references = "")>*/
                 //-------------------------------------------------------------------
                 /// <summary>
                 /// call non-statics like the type class Args
                 /// </summary>
                 /// <param name="assembly_library_code"></param>
                 /// <param name="str_class_name"></param>
                 /// <param name="str_function_name"></param>
                 /// <param name="obj_parameters_array"></param>
                 /// <returns></returns>
            public static dynamic CreateClassInstance(Assembly assembly_library_code, string str_class_name, object[] obj_parameters_array, System.Reflection.BindingFlags int_flags = ( BindingFlags.Public | BindingFlags.Instance ))
                {
                //<diagnostics to see if member names are proper>((((System.Reflection.RuntimeAssembly)assembly_library_code).DefinedTypes).Where(c=>c.FullName.Contains("Includes." + str_class_name)))
                string str_fullname = "Includes." + str_class_name;
                bool bln_ignore_case = false;
                System.Reflection.BindingFlags flags = ( BindingFlags.Public | BindingFlags.Instance );
                var obj_return = assembly_library_code.CreateInstance(str_fullname, bln_ignore_case, flags, null, obj_parameters_array, null, new object[] { });
                return ( obj_return );
                }//</CreateClassInstance(Assembly assembly_library_code, string str_class_name, object[] obj_parameters_array)>
                 //-------------------------------------------------------------------
                 /// <summary>
                 /// Author: Darkstrumn
                 /// Function:  CallFunction call the included functions specified
                 /// </summary>
                 /// <param name="obj_library_code_instance"></param>
                 /// <param name="str_function_name"></param>
                 /// <param name="obj_parameters_array"></param>
                 /// <returns></returns>
            public static object CallFunction(Assembly assembly_library_code, string str_function_name, object[] obj_parameters_array)
                {
                //
                var obj_library_code_instance = assembly_library_code.CreateInstance("Includes." + str_function_name);
                //<diagnostics>Type[] obj_library_types = ((Assembly)assembly_library_code).GetTypes();
                Type type_instance_type = obj_library_code_instance.GetType();
                var method_info = type_instance_type.GetMethod(str_function_name);
                //
                object obj_return = method_info.Invoke(obj_library_code_instance, obj_parameters_array);
                return ( obj_return );
                }//</CallFunction(Object obj_library_code_instance, str_function_name, object[] obj_parameters_array)>
                 //-------------------------------------------------------------------
                 /// <summary>
                 /// Author: Darkstrumn
                 /// Function: GetInclude is an alias to the loading of Includes. Store unboxed object to instance variable and use it with the CallFunction
                 /// function to call the included functions
                 /// </summary>
                 /// <param name="windowHandle"></param>
                 /// <param name="str_include"></param>
                 /// <param name="str_references"></param>
                 /// <returns></returns>
            public static object GetInclude(IntPtr windowHandle, string str_include, string str_references = "")
                {
                str_references = ( str_references.Length != 0 ? str_references : Includes.SupportClasses.Constants.str_default_references );
                string str_code = ScriptEngine.Base64Decode(BFS.ScriptSettings.ReadValue(str_include));
                var obj_include = ScriptEngine.CsCodeAssembler(windowHandle, str_include, str_code, str_references);
                return ( (object)obj_include );
                }//</GetInclude(IntPtr windowHandle, string str_include,string str_references = "")>
                 //-------------------------------------------------------------------
                 /// <summary>
                 /// Author: Darkstrumn
                 /// Function: Intially intended to emulate the way VoiceBot stores it macroscripts this also has the effect of
                 /// shrinking and preserving it textual content as well as making it easy to transport via the web
                 /// large amounts of data. Thus it can be uses as poor-man's compression, so I refactored it out here
                 /// </summary>
                 /// <param name="str_content"></param>
                 /// <returns></returns>
            public static string Base64Encode(string str_content)
                {
                string str_return = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str_content));
                return ( str_return );
                }//</Base64Encode(string str_content)>
                 //-------------------------------------------------------------------
                 /// <summary>
                 /// Author: Darkstrumn
                 /// Function: Intially intended to emulate the way VoiceBot stores it macroscripts this also has the effect of
                 /// shrinking and preserving it textual content as well as making it easy to transport via the web
                 /// large amounts of data. Thus it can be uses as poor-man's compression, so I refactored it out here
                 /// </summary>
                 /// <param name="str_content"></param>
                 /// <returns></returns>
            public static string Base64Decode(string str_content)
                {
                string str_return = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(BFS.ScriptSettings.ReadValue(str_content)));
                return ( str_return );
                }//</Base64Decode(string str_content)>
                 /// <summary>
                 /// Author: Darkstrumn
                 /// Function: LoadInclude is an alias to the loading of includes via local cs file. Store unboxed object to instance variable and use it with the CallFunction
                 /// function to call the included functions
                 /// </summary>
                 /// <param name="windowHandle"></param>
                 /// <param name="str_path"></param>
                 /// <param name="str_references"></param>
                 /// <returns></returns>
            public static object LoadInclude(IntPtr windowHandle, string str_path, string str_references = "")
                {
                object obj_include = null;
                str_references = ( str_references.Length != 0 ? str_references : Includes.SupportClasses.Constants.str_default_references );
                string str_include_path = ( Includes.SupportClasses.Constants.str_default_Include_path + str_path + ".cs" ).Replace("\\\\", "\\");//<<--conditioning:: undo the script compatibility conditioning of the constant = app.path\ScriptExtension\IncludeFile.cs
                                                                                                                                                  //
                try
                    {
                    TextReader tr_include_code = new StreamReader(str_include_path);
                    string str_code = tr_include_code.ReadToEnd().ToString();
                    tr_include_code.Close();
                    //
                    if ( str_code.Length > 0 )
                        { obj_include = ScriptEngine.CsCodeAssembler(windowHandle, str_include_path, str_code, str_references); }
                    else
                        {; }
                    //</if(str_code.Length > 0)>
                    }
                catch {; }
                //</try>
                return ( (object)obj_include );
                }//</GetInclude(IntPtr windowHandle, string str_include,string str_references = "")>
                 //-------------------------------------------------------------------
            }/*</class::ScriptEngine>*/
             //=====================================================================
             //== /ScriptEngine Function Library
             //=====================================================================

        #endregion ScriptEngine
        }/*</namespace::SupportClasses>*/
    //=========================================================================
    //== /Support Classes
    //=========================================================================

    #endregion Support Classes

    //=========================================================================

    #region Speech

    //=========================================================================
    //== Speech Function Library
    //=========================================================================
    /// <summary>
    /// Author: Darkstrumn:\created::160105.21
    /// Function: Speech function library providing expanded functionality, such as voice
    /// recognition, etc.
    /// using System.Speech.Recognition; //<<--needs C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1\\Profile\\Client\\System.Speech.dll
    /// using System.Speech.Synthesis;
    /// </summary>
    public static class Speech
        {
        //---------------------------------------------------------------------
        /// <summary>
        /// Author: Darkstrumn:\created::160105.21
        /// Function: SpeechRecognizer provide voice prompts, where the user can be prompted
        /// aurally to respond verbally and the response is converted to string and
        /// returned for further processing
        /// </summary>
        /// <param name="str_voice_prompt"></param>
        /// <returns></returns>
        public static string SpeechRecognizer(string str_voice_prompt)
            {
            string str_return = "";
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
            Grammar dictationGrammar = new DictationGrammar();
            recognizer.LoadGrammar(dictationGrammar);
            BFS.Speech.TextToSpeech(str_voice_prompt);
            //
            try
                {
                recognizer.SetInputToDefaultAudioDevice();
                RecognitionResult result = recognizer.Recognize();
                if ( result != null ) { str_return = result.Text; } else {; }
                //<diagnostics>BFS.Speech.TextToSpeech(result.Text);
                }
            catch ( InvalidOperationException exception )
                {
                BFS.Dialog.ShowMessageError("Error detected during sound acquisition: {SOURCE} - {MESSAGF}.".Replace("{SOURCE}", exception.Source).Replace("{MESSAGF}", exception.Message));
                }
            finally
                {
                recognizer.UnloadAllGrammars();
                }//</try>
            return ( str_return );
            }//</SpeachRecognizer()>
             //-------------------------------------------------------------------------
        }/*</class Speech>*/
    //=========================================================================
    //== /Speech Function Library
    //=========================================================================

    #endregion Speech

    //=========================================================================

    #region FxLib

    //=========================================================================
    //== FxLib Function Library
    //=========================================================================
    /// <summary>
    /// Author: Darkstrumn:\created::160105.21
    /// Function: define standard library functions - The Toolbox
    /// with the ScriptEngine GetInclude functionality, additional functions can be stored as variables,
    /// and loaded dynamically for common library functionaity
    /// </summary>
    public static class FxLib
        {
        //---------------------------------------------------------------------
        //public static Includes.SupportClasses.Args GetArgs(string str_variable_name)
        public static Dictionary<int, KeyValuePair<string, string>> GetArgs(string str_variable_name)
            {
            Dictionary<int, KeyValuePair<string, string>> args_return = new Dictionary<int, KeyValuePair<string, string>>();
            string str_key;
            string str_value;
            string str_diagnostics = BFS.ScriptSettings.ReadValue(str_variable_name);
            try
                {
                foreach ( string element in BFS.ScriptSettings.ReadValue(str_variable_name).Split('|') )
                    {
                    str_key = element.Split('`')[0];
                    str_value = ( element.Split('`').Length > 1 ? element.Replace(str_key + "`", "") : element );
                    args_return.Add(args_return.Count, new KeyValuePair<string, string>(str_key, str_value));
                    } //</foreach ( string element in BFS.ScriptSettings.ReadValue(str_variable_name).Split('|') )>
                }
            catch ( Exception Error )
                {
                BFS.Dialog.ShowMessageError("!!!GetArgs\\\\Error::" + Error.Message);/*<<--default if not defined*/
                BFS.Speech.TextToSpeech("!!!GetArgs\\\\Error::" + Error.Message);
                }
            return ( args_return );
            }/*</GetArgs(string str_variable_name)>*/
        //---------------------------------------------------------------------
        /// <summary>
        /// Author: Darkstrumn:\created::160105.21
        /// Function: basic serialization: the developer is responsible for documenting the parameter list as this
        /// supports position based args, not exactly name based, but can be expanded to easily.
        /// example:
        /// FxLib.SaveArgs("MyScript",("hello|world").Split('|')); //<<--will save the 2 element array ["hello","world"]
        /// as a serialized string "hello|world" in the registry for later extraction by the intended macro.
        /// </summary>
        public static void SaveArgs(string str_variable_name, string[] arr_str_content)
            {
            string str_content = "";//<<--default to deleting variable value
                                    //
            if ( arr_str_content != null )//>>>>>serialize
                {
                foreach ( string str_item in arr_str_content )
                    {
                    str_content += ( str_content.Length > 0 ? "|" : "" ) + str_item;
                    }//</foreach (string str_item in arr_str_content)>
                }
            else { str_content = "NULL"; }//<<--do nothing
                                          //</>
            BFS.ScriptSettings.WriteValue(str_variable_name, str_content);
            }/*</SaveArgs(string str_variable_name, string[] arr_str_content)>*/
        //---------------------------------------------------------------------
        }/*</class::FxLib>*/
    //=========================================================================
    //== /FxLib Function Library
    //=========================================================================

    #endregion FxLib

    //=========================================================================

    #region Database Classes

    //=========================================================================
    //== Database Classes
    //=========================================================================
    /// <summary>
    /// Author: Darkstrumn:\created::160112.12
    /// Function: provides DB functionality for use with macro scripts, improved IPC finctionality over the use of the registry for complexe macro logic
    /// >>>>>if voicebot complains that the provider is not register on the local machine, then instll the access engine for your system [32|64]bit:: https://www.microsoft.com/en-us/download/details.aspx?id=13255
    /// using System.Data.OleDb; //<<--System.Data.DataSetExtensions.dll is needed
    /// </summary>
    public static class DBS
        {
        //---------------------------------------------------------------------
        /// <summary>
        /// Author: Darkstrumn:\created::160112.12
        /// Function: type extension for true null handling
        /// </summary>
        /// <param name="obj_field"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj_field)
            {
            bool bln_return = ( ( obj_field == null ) || ( obj_field == DBNull.Value ) );
            return ( bln_return );
            }//</IsDBNull(object obj_field)>
        //---------------------------------------------------------------------
        /// <summary>
        /// Author: Darkstrumn:\created::160112.12
        /// Function: Query provides basic DB query functionality (mssql\\access typically), returns Datatable of results
        /// </summary>
        /// <param name="str_query"></param>
        /// <param name="str_connectionstring"></param>
        /// <returns></returns>
        public static DataTable Query(string str_query, string str_connectionstring)
            {
            DataTable dt_result = new DataTable();
            str_connectionstring = ( str_connectionstring.Length > 0 ? str_connectionstring : Includes.SupportClasses.Constants.str_default_connection_string ); // "\\ScriptExtension\\" <<--this must be created in voicebot install folder and the .mdb files should be stored in here so the macroscripts have access to a standard location.
            //
            try
                {
                //>>>>>Open OleDb Connection using MSAccess type localdb file
                OleDbConnection db_connection = new OleDbConnection();
                db_connection.ConnectionString = str_connectionstring;
                db_connection.Open();
                //>>>>>Execute Queries
                OleDbCommand db_context = db_connection.CreateCommand();
                db_context.CommandText = str_query;
                OleDbDataReader db_reader = db_context.ExecuteReader(CommandBehavior.CloseConnection); //>>>>>close connection after complete
                //>>>>>Load the result into a DataTable
                dt_result.Load(db_reader);
                }
            catch ( Exception error )
                {
                String str_error_message = "Query::Commander, OLEDB Connection FAILED: {ERRORMSG}".Replace("{ERRORMSG}", error.Message);
                //<replaced>BFS.Speech.TextToSpeech(str_error_message);
                BFS.Dialog.ShowMessageError(str_error_message);
                }//</try>
            return ( dt_result );
            }
        }/*</class::DBS>*/
         //=========================================================================
         //== /Database Classes
         //=========================================================================

    #endregion Database Classes

    //=========================================================================
    }/*</namespace::Includes>*/