///<summary>
///Custom include that shows how to include the base library for additional functionality and tie-in to the rest of the macroscript when itself is an include
///ShipModel attempts to create an ORM for the ships features that can be used to keep track of ship state as the macros execute and change that state.
///"manual overrides" will ofcourse upset this tracking by causing state-changes that are not tracked via the macro, 
///</summary>
using System;
using System.Collections.Generic;//<<--needed for Dictionary
using System.Data; //<<--in references
using System.Data.OleDb; //<<--provided by System.Data.DataSetExtensions.dll
using System.Diagnostics;//<<--needed for debug (visual studio IDE only)
using System.Linq;//<<--provided by C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1\\Profile\\Client\\System.Data.Linq.dll
using System.Threading;
using System.Threading.Tasks;
using System.Drawing; //<<--in references
using System.Dynamic;
using System.IO;//<<--text file io for loading includes files
using System.Management; //<<--in references
using System.Speech.Recognition;//<<-- provided by C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1\\Profile\\Client\\System.Speech.dll
using System.Speech.Synthesis;
using System.Web; //<<--in references
using System.Windows; //<<--in references
using System.Xml; //<<--in references
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text; //<<--provided by mscorlib.dll
using Microsoft.CSharp; //<<--in system.dll
//<references:>System.Core.dll |System.Data.dll | System.dll | System.Drawing.dll | System.Management.dll | System.Web.dll | System.Windows.Forms.dll | System.Xml.dll | mscorlib.dll | C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Profile\Client\System.Speech.dll | C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Profile\Client\System.Data.Linq.dll

namespace Includes
    {
    //=========================================================================
    #region Ship Model Classes
    //=========================================================================
    //== Ship Model Classes
    //=========================================================================
    /// <summary>
    /// Author: Darkstrumn:\created::160112.12
    /// Function: model ship system state. voicebot stores vars in registry as a
    /// string, so we will use one string to store the state of the ship, vs. multiple
    /// </summary>
    public static class ShipModel
        {
        //---------------------------------------------------------------------
        public static void ResetShipState()
            {
            Dictionary<int, KeyValuePair<string,string>> args_ship_state = Includes.FxLib.GetArgs("ship_state");
            string[,] arr_ship_state = new string[,] {
    { "shipName", "DIN Ka'pla" } //<<--name
    ,{"cargoScoop","0" } //<<--cargo scoop deployed
    ,{"landingGear","0" } //<<--landing gear deployed
    ,{"externalLights","0" } //<<--ship lights deployed
    ,{"flightAssist","1" } //<<--flight assist
    ,{"hardpoints","0" } //<<--hardpoints deployed
    ,{"silentRunning","0" } //<<--silent running
    ,{"activeFiregroup","0" } //<<--active fire group
    ,{"shipType","8" } //<<--ship type id (used to set properties such as number of slots for Modules and such
    ,{"modules","20" } //<<--modules (number of module slots)
    ,{"firegoups","5" } //<<--fire groups (number of fire groups, default 5)
    ,{"refineryBins","5" } //<<--refinary present (value is number of hopper bins avilable)
    ,{"systemsPower","2" } //<<--systems (power distribution state)
    ,{"enginesPower","2" } //<<--engines (power distribution state)
    ,{"weaponsPower","2" } //<<--weapons (power distribution state)
    ,{"turretMode","2" } //<<--turret weapons mode (1=fixed, 2=target only, 3=fire at will (default 2))
    ,{"beacon","0" } //<<--beacon (0=off, 1=wing (defult 0))
    ,{"confirmation","0" } //<<--expecting confirmation (0=no, 1=yes) used to ignore confirmation response triggers if unexpected
    ,{"jettison","0" } //<<--jettison all cargo (0=no, 1=confirming, 2= commit)
    };
            string[,] arr_ship_screens_state = new string[,] {
    { "1", "0"} //<<--screen1-target tab(leftRight) index
    ,{ "1.1.y", "2"} //<<--subscreen 1 tab 1\\Navigation upDown selection index (reservered for future use)
    ,{ "1.1.x", "0"} //<<--subscreen 1 tab 1\\Navigation leftRight selection index (reservered for future use)
    ,{ "1.2.y", "0"} //<<--subscreen 1 tab 2\\Transactions upDown selection index (reservered for future use
    ,{ "1.2.x", "0"} //<<--subscreen 1 tab 2\\Transactions leftRight selection index (reservered for future use)
    ,{ "1.3.y", "0"} //<<--subscreen 1 tab 3\\Contacts upDown selection index (reservered for future use
    ,{ "1.3.x", "0"} //<<--subscreen 1 tab 3\\Contacts leftRight selection index (reservered for future use)
    ,{ "1.4.y", "0"} //<<--subscreen 1 tab 4\\Inventory upDown selection index (reservered for future use
    ,{ "1.4.x", "0"} //<<--subscreen 1 tab 4\\Inventory leftRight selection index (reservered for future use)
    ,{ "2", "0"} //<<--screen2-comms tab(leftRight) index
    ,{ "2.1.y", "0"} //<<--subscreen 2 tab 1\\Local upDown selection index (reservered for future use)
    ,{ "2.1.x", "0"} //<<--subscreen 2 tab 1\\Local leftRight selection index (reservered for future use)
    ,{ "2.2.y", "0"} //<<--subscreen 2 tab 2\\Voice upDown selection index (reservered for future use)
    ,{ "2.2.x", "0"} //<<--subscreen 2 tab 2\\Voice leftRight selection index (reservered for future use)
    ,{ "2.3.y", "0"} //<<--subscreen 2 tab 3\\Email upDown selection index (reservered for future use)
    ,{ "2.3.x", "0"} //<<--subscreen 2 tab 3\\Email leftRight selection index (reservered for future use)
    ,{ "2.4.y", "0"} //<<--subscreen 2 tab 4\\Wing upDown selection index (reservered for future use)
    ,{ "2.4.x", "0"} //<<--subscreen 2 tab 4\\Wing leftRight selection index (reservered for future use)
    ,{ "2.5.y", "0"} //<<--subscreen 2 tab 5\\Prefs upDown selection index (reservered for future use)
    ,{ "2.5.x", "0"} //<<--subscreen 2 tab 5\\Prefs leftRight selection index (reservered for future use)
    ,{ "3", "0"} //<<--screen3-role tab(leftRight) index
    ,{ "3.1.y", "0"} //<<--subscreen 3 tab 1\\Ship upDown selection index (reservered for future use)
    ,{ "3.1.x", "0"} //<<--subscreen 3 tab 1\\Ship leftRight selection index (reservered for future use)
    ,{ "3.2.y", "0"} //<<--subscreen 3 tab 2\\SRV upDown selection index (reservered for future use)
    ,{ "3.2.x", "0"} //<<--subscreen 3 tab 2\\SRV leftRight selection index (reservered for future use)
    ,{ "4", "0"} //<<--screen4-systems tab(leftRight) index
    ,{ "4.1.y", "0"} //<<--subscreen 4 tab 1\\Status upDown selection index (reservered for future use)
    ,{ "4.1.x", "0"} //<<--subscreen 4 tab 1\\Status leftRight selection index (reservered for future use)
    ,{ "4.2.y", "0"} //<<--subscreen 4 tab 2\\Modules upDown selection index (reservered for future use)
    ,{ "4.2.x", "0"} //<<--subscreen 4 tab 2\\Modules leftRight selection index (reservered for future use)
    ,{ "4.3.y", "0"} //<<--subscreen 4 tab 3\\Inventory upDown selection index (reservered for future use)
    ,{ "4.3.x", "0"} //<<--subscreen 4 tab 3\\Inventory leftRight selection index (reservered for future use)
    ,{ "4.4.y", "0"} //<<--subscreen 4 tab 4\\FireGroups upDown selection index (reservered for future use)
    ,{ "4.4.x", "0"} //<<--subscreen 4 tab 4\\FireGroups leftRight selection index (reservered for future use)
    ,{ "4.5.y", "0"} //<<--subscreen 4 tab 5\\Functions upDown selection index (reservered for future use)
    ,{ "4.5.x", "0"} //<<--subscreen 4 tab 5\\Functions leftRight selection index (reservered for future use)
    };
            int int_loop = 0;
            int int_loop_size = arr_ship_state.GetLength(0);
            string str_ship_state = "";
            for ( int_loop = 0; int_loop < int_loop_size; int_loop++ )
                {
                str_ship_state += ( str_ship_state.Length > 0 ? "|" : "" ) + arr_ship_state[int_loop, 0] + "," + arr_ship_state[int_loop, 1];//<<--simple-serialization
                }//</for ( int_loop = 0; int_loop < int_loop_size; int_loop++ )>
            Includes.FxLib.SaveArgs("ship_state", str_ship_state.Split('|'));
            }//</_ResetShipState()>
        //---------------------------------------------------------------------
        public static string GetState(string str_property)
            {
            string str_return = "";
            string str_key = "";
            bool bln_found = false;
            Dictionary<int, KeyValuePair<string,string>> args_ship_state = Includes.FxLib.GetArgs("ship_state");
            //<diagnostics>BFS.Speech.TextToSpeech("GetState::Number of data elements to scan is {NUM}.".Replace("{NUM}", args_ship_state.Count.ToString()));
            foreach (KeyValuePair<int,KeyValuePair<string,string>> row in args_ship_state)
                {
                str_key = row.Value.Key;
                if ( str_key == str_property )
                    {
                    bln_found = true;
                    str_return = row.Value.Value;
                    break;//<<--breach the loop
                    }
                else {; }
                //</if (str_key == str_property )>
                }//</foreach ( string row in args_ship_state )>
            //
            if ( bln_found )
                { BFS.Speech.TextToSpeech("GetState::state property '{PROPERTY}' found:{VALUE}. ".Replace("{PROPERTY}", str_property).Replace("{VALUE}", str_return)); }
            else
                { BFS.Speech.TextToSpeech("GetState::state property '{PROPERTY}' not found! ".Replace("{PROPERTY}", str_property)); }
            //</if ( !bln_found )>
            return ( str_return );
            }//</GetState(string str_property)>
	    //---------------------------------------------------------------------
	    public static void SetState(string str_property, string str_value)
		    {
		    string str_key = "";
		    int int_index = -1;
		    bool bln_found = false;
		    string str_ship_state = "";
		    string str_old_state = "";
		    string str_new_state = "";
		    Dictionary<int, KeyValuePair<string,string>> args_ship_state = Includes.FxLib.GetArgs("ship_state");
		    //>>>>>ensure we have data, else initialize
		    if(args_ship_state.Count == 0)
			    {
			    ShipModel.ResetShipState();//<<--init
			    args_ship_state = Includes.FxLib.GetArgs("ship_state");//<<--reload
			    }
		    else {; }//>>>>>do nothing
		    //</if(if(args_ship_state.Count == 0))>
		    Dictionary<int, KeyValuePair<string,string>> args_new_ship_state = Includes.FxLib.GetArgs("ship_state");
		    //<diagnostics>BFS.Speech.TextToSpeech("GetState::Number of data elements to scan is {NUM}.".Replace("{NUM}", args_ship_state.Count.ToString()));
		    foreach (KeyValuePair<int, KeyValuePair<string,string>> row in args_ship_state)
			    {
			    int_index++;
			    str_key = row.Value.Key;
			    str_old_state = args_ship_state[int_index].Value;
			    //
			    if ( str_key == str_property )
				    {
				    bln_found = true;
				    str_new_state = str_value;
				    }
			    else
				    {
				    str_new_state = str_old_state;
			        }
			    //</if (str_key == str_property )>
			    str_ship_state += ( str_ship_state.Length > 0 ? "|" : "" ) + str_key + "," + str_new_state;
			    }//</foreach ( string row in args_ship_state )>
		    //
		    if ( !bln_found )//>>>>>create
			    {
			    args_ship_state.Add(args_ship_state.Count,new KeyValuePair<string,string>(str_key, str_value));
			    }
		    else//>>>>>continue to save
			    {; }
		    //</if ( bln_found )>
		    BFS.ScriptSettings.WriteValue("ship_state", str_ship_state);
		    }//</SetState(string str_property,string str_value)>
	    //---------------------------------------------------------------------
        }/*</class::ShipModel>*/
    //=========================================================================
    //==/Support Classes
    //=========================================================================
    #endregion
    //=========================================================================
   }/*</namespace::Include>*/
//=============================================================================
#region Includes
//=============================================================================
//== Includes Classes
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
    #region Support Classes
    //=========================================================================
    //== Support Classes
    //=========================================================================
    /// <summary>
    /// Author: Darkstrumn:\created::160115.03
    /// Function: Support classes such are custom datatypes, etc.
    /// </summary>
    namespace SupportClasses
        {
        //=====================================================================
        /// <summary>
        /// Author: Darkstrumn
        /// Function: Provides central location for common or default global values, pseudo-constants
        /// </summary>
        public static class Constants
            {
            //-----------------------------------------------------------------
            public static string str_default_references = "System.Core.dll |System.Data.dll | System.dll | System.Drawing.dll | System.Management.dll | System.Web.dll | System.Windows.Forms.dll | System.Xml.dll | mscorlib.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1\\Profile\\Client\\System.Speech.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1\\Profile\\Client\\System.Data.Linq.dll".Replace("\\","\\\\");
            public static string str_default_connection_string = "Provider='Microsoft.ACE.OLEDB.12.0'; Data Source='{PATH}LogBook.mdb'; Persist Security Info=False".Replace("{PATH}", (BFS.General.GetAppInstallPath() + "\\ScriptExtension\\")).Replace("\\","\\\\");
            /*
            The following path will likely need to be created to store any includes you make, as a common place to put them.
            The dir can be created using the following cmd from the command prompt:
            mkdir %LOCALAPPDATA%\VoiceBot\ScriptExtension\
            The path created should be something similar the following:
            C:\Users\YOUR_USER_NAME_HERE\AppData\Local\VoiceBot\ScriptExtension\
            */
            public static string str_default_Include_path = (BFS.General.GetAppInstallPath() != "C:\\mnt\\W\\DevCore\\ProtoLab\\dotnet\\dotnet2k15\\Projects\\VoiceBotScript\\VoiceBotScript\\bin\\Debug") ?
            (Environment.GetEnvironmentVariable("LOCALAPPDATA")+"\\VoiceBot\\ScriptExtension\\").Replace("\\","\\\\")
            : "C:\\mnt\\DevCore\\ProtoLab\\dotnet\\dotnet2k15\\Projects\\VoiceBotScript\\VoiceBotScript\\ScriptExtension\\".Replace("\\","\\\\");

            //-----------------------------------------------------------------
            }/*</class Constants>*/
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
          /// <summary>
          /// Author: Darkstrumn
          /// Function: Eval takes provided CS source and attempts to compile code, then returns an instance object of the assembly
          /// viable for loading aux scripts to form includes or dynamic code loading
          /// </summary>
          /// <param name="windowHandle"></param>
          /// <param name="str_source_cs"></param>
          /// <param name="str_references"></param>
          /// <returns></returns>
          public static object Eval( IntPtr windowHandle,string str_code_block_name, string str_source_cs, string str_references = "")
            {  
            object obj_return = null;
            str_references = (str_references.Length != 0 ? str_references : Includes.SupportClasses.Constants.str_default_references);
            //
            //CodeDomProvider provider = new CSharpCodeProvider(new Dictionary<String, String>{{ "CompilerVersion","v4.6.1" }});
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            //
            CompilerParameters cp_compiler_params = new CompilerParameters();
            //>>>>>compileing options to build code
            cp_compiler_params.CompilerOptions = "/t:library";//<<--craft as library dll
            cp_compiler_params.GenerateExecutable  = false;//<<--do not craft executable
            cp_compiler_params.GenerateInMemory = true;//<<--output in memmory vs output file
            cp_compiler_params.TreatWarningsAsErrors = false;//<<--warning handling
            str_references += (str_references.Length > 0 ? "|" : "") + "C:\\Program Files (x86)\\VoiceBot\\VoiceBot.exe";
            foreach(string str_reference in str_references.Split('|'))
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
            if(cr_result.Errors.Count > 0)//>>>>>report error, and fail
              {
              foreach(CompilerError CompErr in cr_result.Errors)
                {
                string str_line = sb_script_core.ToString().Split('\n')[CompErr.Line - 1];
                //BFS.Dialog.ShowMessageError("Source:\n{SOURCE}".Replace("{SOURCE}",sb_script_core.ToString()) );
                BFS.Dialog.ShowMessageError("Eval\\\\ERROR: Source {NAME}".Replace("{NAME}",str_code_block_name));
                BFS.Dialog.ShowMessageError("Eval\\\\ERROR: Line number {LINE}, Error Number: {NUMBER}, '{TEXT}'".Replace("{LINE}",CompErr.Line.ToString()).Replace("{NUMBER}",CompErr.ErrorNumber).Replace("{TEXT}",CompErr.ErrorText) );
                BFS.Dialog.ShowMessageError("Eval\\\\ERROR: Code on line number {NUMBER} => '{LINE}'".Replace("{NUMBER}",CompErr.Line.ToString()).Replace("{LINE}",str_line) );
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
            return(obj_return);
            }/*</Eval( IntPtr windowHandle, string str_source_cs, string str_references = "")>*/
          //-------------------------------------------------------------------
          /// <summary>
          /// call non-statics like the type class Args
          /// </summary>
          /// <param name="assembly_library_code"></param>
          /// <param name="str_class_name"></param>
          /// <param name="str_function_name"></param>
          /// <param name="obj_parameters_array"></param>
          /// <returns></returns>
          public static dynamic CreateClassInstance(Assembly assembly_library_code, string str_class_name, object[] obj_parameters_array, System.Reflection.BindingFlags int_flags = (BindingFlags.Public | BindingFlags.Instance))
            {
            //<diagnostics to see if member names are proper>((((System.Reflection.RuntimeAssembly)assembly_library_code).DefinedTypes).Where(c=>c.FullName.Contains("Includes." + str_class_name)))
            string str_fullname = "Includes." + str_class_name;
            bool bln_ignore_case = false;
            System.Reflection.BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance);
            var obj_return = assembly_library_code.CreateInstance( str_fullname, bln_ignore_case, flags,null, obj_parameters_array, null, new object[] {});
  	        return(obj_return);
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
  	        return(obj_return);
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
          public static object GetInclude(IntPtr windowHandle, string str_include,string str_references = "")
            {
            str_references = (str_references.Length != 0 ? str_references : Includes.SupportClasses.Constants.str_default_references);
            string str_code = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(BFS.ScriptSettings.ReadValue(str_include)));
            var obj_include = ScriptEngine.Eval(windowHandle, str_include, str_code,str_references);
            return((object)obj_include);
            }//</GetInclude(IntPtr windowHandle, string str_include,string str_references = "")>
          //-------------------------------------------------------------------
          /// <summary>
          /// Author: Darkstrumn
          /// Function: LoadInclude is an alias to the loading of includes via local cs file. Store unboxed object to instance variable and use it with the CallFunction
          /// function to call the included functions
          /// </summary>
          /// <param name="windowHandle"></param>
          /// <param name="str_path"></param>
          /// <param name="str_references"></param>
          /// <returns></returns>
          public static object LoadInclude(IntPtr windowHandle, string str_path,string str_references = "")
            {
            object obj_include = null;
            str_references = (str_references.Length != 0 ? str_references : Includes.SupportClasses.Constants.str_default_references);
            string str_include_path = (Includes.SupportClasses.Constants.str_default_Include_path + str_path + ".cs").Replace("\\\\","\\");//<<--conditioning:: undo the script compatibility conditioning of the constant = app.path\ScriptExtension\IncludeFile.cs
            //   
            try
                {
                TextReader tr_include_code = new StreamReader(str_include_path );
                string str_code = tr_include_code.ReadToEnd().ToString();
                tr_include_code.Close();
                //
                if(str_code.Length > 0)
                    {obj_include = ScriptEngine.Eval(windowHandle, str_include_path, str_code,str_references);}
                else
                    {;}
                //</if(str_code.Length > 0)>
                }
            catch {;}
            //</try>
            return((object)obj_include);
            }//</GetInclude(IntPtr windowHandle, string str_include,string str_references = "")>
          //-------------------------------------------------------------------
          }/*</class::ScriptEngine>*/
        //=====================================================================
        //== /ScriptEngine Function Library
        //=====================================================================
        #endregion
        //=====================================================================
        }/*</namespace::SupportClasses>*/
    //=========================================================================
    //== /Support Classes
    //=========================================================================
    #endregion
    //=========================================================================
    /// <summary>
    /// Author: Darkstrumn 160120.01
    /// Function: faux-namespace\\alias includes via delegation. kinda messy, but "re-integrates" the namespace
    /// and cleanliness of code down the line the working code is housed in the include,
    /// but are craft delagation aliases for any items we want to, in this case the
    /// FxLib.
    /// </summary>
     public static class FxLib
        {
        //>>>>>load includes, then wireup aliases to assembly
        public static Type type_FxLib = ((Assembly)Includes.SupportClasses.ScriptEngine.LoadInclude(new IntPtr(),"Include_DarkLibs",Includes.SupportClasses.Constants.str_default_references)).GetTypes().Where( x => x.FullName.Contains("Includes.FxLib")).ToArray<Type>()[0];
        //>>>>>appoint delagates (signatures)
        //public delegate Includes.SupportClasses.Args GetArgsDelegate(string str_variable_name);
        public delegate Dictionary<int,KeyValuePair<string,string>> GetArgsDelegate(string str_variable_name);
        public delegate void SaveArgsDelegate(string str_variable_name, string[] arr_content);
        //>>>>>staff them
        public static GetArgsDelegate GetArgs = (GetArgsDelegate)Delegate.CreateDelegate(typeof(GetArgsDelegate), ((type_FxLib.GetMethods().Where(x => x.Name == "GetArgs" ).ToArray<MethodInfo>()))[0]);
        public static SaveArgsDelegate SaveArgs = (SaveArgsDelegate)Delegate.CreateDelegate(typeof(SaveArgsDelegate), ((type_FxLib.GetMethods().Where(x => x.Name == "SaveArgs" ).ToArray<MethodInfo>()))[0]);
        }/*</class::FxLib>*/
    //=========================================================================
    /// <summary>
    /// Author: Darkstrumn 160120.01
    /// Function: faux-namespace\\alias includes via delegation. kinda messy, but "re-integrates" the namespace
    /// and cleanliness of code down the line the working code is housed in the include,
    /// but are craft delagation aliases for any items we want to, in this case the
    /// Speech.
    /// </summary>
     public static class Speech
        {
        //>>>>>load includes, then wireup aliases to assembly
        static Type type_Speech = ((Assembly)Includes.SupportClasses.ScriptEngine.LoadInclude(new IntPtr(),"Include_DarkLibs",Includes.SupportClasses.Constants.str_default_references)).GetTypes().Where( x => x.FullName.Contains("Includes.Speech")).ToArray<Type>()[0];
        //>>>>>appoint delagates (signatures)
        public delegate string SpeechRecognizerDelegate(string str_voice_prompt);
        //>>>>>staff them
        public static SpeechRecognizerDelegate SpeechRecognizer = (SpeechRecognizerDelegate)Delegate.CreateDelegate(typeof(SpeechRecognizerDelegate), ((type_Speech.GetMethods().Where(x => x.Name == "SpeechRecognizer" ).ToArray<MethodInfo>()))[0]);
        }/*</class::ShipModel>*/
    //=========================================================================
    /// <summary>
    /// Author: Darkstrumn 160120.01
    /// Function: faux-namespace\\alias includes via delegation. kinda messy, but "re-integrates" the namespace
    /// and cleanliness of code down the line the working code is housed in the include,
    /// but are craft delagation aliases for any items we want to, in this case the
    /// DBServices.
    /// </summary>
     public static class DBS
        {
        //>>>>>load includes, then wireup aliases to assembly
        static Type type_DBS = ((Assembly)Includes.SupportClasses.ScriptEngine.LoadInclude(new IntPtr(),"Include_DarkLibs",Includes.SupportClasses.Constants.str_default_references)).GetTypes().Where( x => x.FullName.Contains("Includes.DBS")).ToArray<Type>()[0];
        //>>>>>appoint delagates (signatures)
        public delegate DataTable QueryDelegate(string str_query, string str_connectionstring);
        //>>>>>staff them
        public static QueryDelegate Query = (QueryDelegate)Delegate.CreateDelegate(typeof(QueryDelegate), ((type_DBS.GetMethods().Where(x => x.Name == "Query" ).ToArray<MethodInfo>()))[0]);
        }/*</class::DBS>*/
    //=========================================================================
    //== /Support Classes
    //=========================================================================
    }/*</namespace Includes>*/
//=============================================================================
//== /Includes Classes
//=============================================================================
#endregion
//=============================================================================
