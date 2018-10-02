using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoiceBotScript2
  {
  //*****************************************************************************
  /// <summary>
  /// Author: Darkstrumn:\created::160107.13
  /// Function: The Aural-OS Core is the workhorse of the macroscript system.
  ///           The system is typically 2 part, the caller, handles the interface
  ///           side of the system, doing random response, and such, then sends
  ///           the requested actions to the AOSCore script via IPC using variables
  ///           that are then executed from this script.
  ///           This systems allows the core logic to be in one script callable by
  ///           other scripts, easy to maintain, and add to.
  /// <summary>
  using System;
  using System.Collections.Generic;//<<--needed for Dictionary
  using System.Data; //<<--in references
  using System.Data.OleDb; //<<--provided by System.Data.DataSetExtensions.dll
  using System.Diagnostics;//<<--needed for debug (visual studio IDE only)
  using System.Linq;//<<--provided by C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Data.Linq.dll
  using System.Threading;
  using System.Threading.Tasks;
  using System.Drawing; //<<--in references
  using System.IO;//<<--text file io for loading includes files
  using System.Management; //<<--in references
  using System.Speech.Recognition;//<<-- provided by C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Speech.dll
  using System.Speech.Synthesis;
  using System.Web; //<<--in references
  using System.Windows; //<<--in references
  using System.Xml; //<<--in references
  using System.CodeDom.Compiler;
  using System.Reflection;
  using System.Text; //<<--provided by mscorlib.dll
  using Microsoft.CSharp; //<<--in system.dll
                          //<references:>System.Core.dll |System.Data.dll | System.dll | System.Drawing.dll | System.Management.dll | System.Web.dll | System.Windows.Forms.dll | System.Xml.dll | mscorlib.dll | C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\System.Speech.dll | C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\System.Data.Linq.dll
                          //=============================================================================
  #region main
  //=============================================================================
  //==main
  //=============================================================================
  public static class VoiceBotScript
    {
    #region private vars
    private static string str_script_name = "AOSCore";
    #endregion
    //---------------------------------------------------------------------------
    /// <summary>
    /// Author: Darkstrumn:\created::160107.13
    /// Function: The Aural-OS Core is the workhorse of the macroscript system.
    ///           The system is typically 2 part, the caller, handles the interface
    ///           side of the system, doing random response, and such, then sends
    ///           the requested actions to the AOSCore script via IPC using variables
    ///           that are then executed from this script.
    ///           This systems allows the core logic to be in one script callable by
    ///           other scripts, easy to maintain, and add to.
    /// <summary>
    /// <param name="windowHandle"></param>
    public static void Run(IntPtr windowHandle)
      {
      string ipc_var_name = str_script_name + "_ipc";
      //>>>>>IPC\\Argument data can be passed forward from macro to macro using vars.
      Dictionary<int, KeyValuePair<string, string>> args = new Dictionary<int, KeyValuePair<string, string>>();
      try
        {
        if( ((Dictionary<int, KeyValuePair<string, string>>)Includes.FxLib.GetArgs(ipc_var_name)).Count > 0 )
          {
          args = (Dictionary<int, KeyValuePair<string, string>>)Includes.FxLib.GetArgs(ipc_var_name);
          BFS.Speech.TextToSpeech("The number of arguments found was " + args.Count.ToString());
          foreach( KeyValuePair<int, KeyValuePair<string, string>> arg in (Dictionary<int, KeyValuePair<string, string>>)args )
            {
            BFS.Speech.TextToSpeech("argument {KEY} is {VALUE}.".Replace("{KEY}", arg.Key.ToString()).Replace("{VALUE}", arg.Value.ToString()));
            }
          }
        else {; }
        //</if ( Includes.FxLib.GetArgs(ipc_var_name).Count > 0 )>
        }
      catch( Exception error )
        {
        BFS.Dialog.ShowMessageError(error.InnerException.Message);
        }
      //<diagnostics>try { if ( Includes.FxLib.GetArgs(ipc_var_name).Count > 0 ) { args = Includes.FxLib.GetArgs(ipc_var_name); } else {; }}catch(Exception error) {BFS.Dialog.ShowMessageError(" Includes.FxLib.GetArgs Includes-Error::{MESSAGE}".Replace("{MESSAGE}",error.Message));}
      int int_numParams = (args.Count > 2 ? (args.Count - 2) : 0); //<<--used for IPC argument handling
      int int_num_subparams;
      int int_expected_num;
      string str_caller = (args.Count > 2 ? args[0].Value : ""); //<<--if called using IPC, this will be the calling module
      string str_method;
      string ship_state;
      string[] arr_arguments;
      string str_arguments;//<<--diagnostic alias
                           //
      if( args.Count > 0 )//>>>>>we have args, parse and store
        {
        //>>>>>process based on IPC data
        for( var intLoop = 1 ; intLoop < (2 + int_numParams) ; intLoop++ )
          {
          str_arguments = args[intLoop].Value;
          arr_arguments = str_arguments.Split(',');
          str_method = args[intLoop].Key;//<<--alias
          int_num_subparams = (arr_arguments.Length);
          //>>>>>handle requested methods
          switch( str_method.ToLower() )
            {
            case "tts":
              int_expected_num = 1;
              //>>>>>arr_arguments[0]; //<<--message to speak
              if( int_num_subparams == int_expected_num )//>>>>>execute
                {
                BFS.Speech.TextToSpeech(arr_arguments[0]);
                }
              else //>>>>>warn
                {
                VoiceArgcError(str_method.ToUpper(), int_num_subparams, int_expected_num);
                }//</if ( (arr_arguments.Length - 1) == 2)>
              break;
            case "initshipstate":
              Includes.ShipModel.ResetShipState();
              break;
            case "getshipstate":
              int_expected_num = 1;
              //>>>>>arr_arguments[0]; //<<--var to get
              if( int_num_subparams == int_expected_num )//>>>>>execute
                {
                if( BFS.ScriptSettings.ReadValue("ShipState").Length < 1 ) { Includes.ShipModel.ResetShipState(); } else {; }//<<--init if not present
                ship_state = Includes.ShipModel.GetState(arr_arguments[0]);
                }
              else //>>>>>warn
                {
                VoiceArgcError(str_method.ToUpper(), int_num_subparams, int_expected_num);
                }//</if ( (arr_arguments.Length - 1) == 2)>
              break;
            case "setshipstate":
              int_expected_num = 2;
              //>>>>>arr_arguments[0]; //<<--var to set
              //>>>>>arr_arguments[1]; //<<--value to set var to
              if( int_num_subparams == int_expected_num )//>>>>>execute
                {
                if( BFS.ScriptSettings.ReadValue("ShipState").Length < 1 ) { Includes.ShipModel.ResetShipState(); } else {; }//<<--init if not present
                Includes.ShipModel.SetState(arr_arguments[0], arr_arguments[1]);
                }
              else //>>>>>warn
                {
                VoiceArgcError(str_method.ToUpper(), int_num_subparams, int_expected_num);
                }//</if ( (arr_arguments.Length - 1) == 2)>
              break;
            case "set":
              int_expected_num = 2;
              //>>>>>arr_arguments[0]; //<<--var to set
              //>>>>>arr_arguments[1]; //<<--value to set var to
              if( int_num_subparams == int_expected_num )//>>>>>execute
                {
                Includes.FxLib.SaveArgs(arr_arguments[0], arr_arguments[1].Split('|'));
                }
              else //>>>>>warn
                {
                VoiceArgcError(str_method.ToUpper(), int_num_subparams, int_expected_num);
                }//</if ( (arr_arguments.Length - 1) == 2)>
              break;
            case "sendkeys":
              int_expected_num = 1;
              //>>>>>arr_arguments[0]; //<<--key-sequence to send
              if( int_num_subparams == int_expected_num )//>>>>>execute
                {
                BFS.Input.SendKeys(arr_arguments[0]);
                }
              else //>>>>>warn
                {
                VoiceArgcError(str_method.ToUpper(), int_num_subparams, int_expected_num);
                }//</if ( (arr_arguments.Length - 1) == 2)>
              break;
            default:
              break;
            }//</switch>
          }//</for(var intLoop = 0; intLoop < args.Count; intLoop++)>
        }
      else
        { /*BFS.Speech.TextToSpeech("No IPC data found.")*/; }
      //</if(int_argc > 0)>
      //
      if( args.Count != 0 )
        { Includes.FxLib.SaveArgs(ipc_var_name, null); }//<<--"delete" the value (I think the registry key remains...)
      else
        {; }
      //</if ( args.Count != 0 )>
      }/*</Run(IntPtr windowHandle)>*/
    //-------------------------------------------------------------------------
    /// <summary>
    /// when the construct is used with IPC subcommands, this is the arugments # error handler
    /// </summary>
    /// <param name="str_method"></param>
    /// <param name="int_num_args"></param>
    /// <param name="int_expected_num"></param>
    public static void VoiceArgcError(string str_method, int int_num_args, int int_expected_num)
      {
      /*Customize audio error as desired*/
      BFS.Speech.TextToSpeech(str_script_name + " Error detected in {METHOD}" + str_method + " parameters, number of arguments provided was {NUM}".Replace("{METHOD", str_method).Replace("NUM}", int_num_args.ToString()));
      BFS.Speech.TextToSpeech("Number of arguments expected is {NUM}".Replace("NUM}", int_expected_num.ToString()));
      }/*</VoiceArgcError(string str_method, int int_num_args, int int_expected_num)>*/
       //-------------------------------------------------------------------------
    }/*</class::VoiceBotScript>*/
     //=============================================================================
     //==/main
     //=============================================================================
  #endregion
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
  /// Assembly assem_Speech = (Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(),"Include_DarkLibs",Includes.VoiceBotSupportClasses.Constants.str_default_references)).GetTypes().Where( x => x.FullName.Contains("Includes.Speech");
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
    namespace VoiceBotSupportClasses
      {
      //=====================================================================
      /// <summary>
      /// Author: Darkstrumn
      /// Function: Provides central location for common or default global values, pseudo-constants
      /// </summary>
      public static class Constants
        {
        //-----------------------------------------------------------------
        public static string str_default_references = "System.Core.dll |System.Data.dll | System.dll | System.Drawing.dll | System.Management.dll | System.Web.dll | System.Windows.Forms.dll | System.Xml.dll | mscorlib.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Speech.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Data.Linq.dll".Replace("\\", "\\\\");
        public static string str_default_connection_string = "Provider='Microsoft.ACE.OLEDB.12.0'; Data Source='{PATH}LogBook.mdb'; Persist Security Info=False".Replace("{PATH}", (BFS.General.GetAppInstallPath() + "\\ScriptExtension\\")).Replace("\\", "\\\\");
        /*
        The following path will likely need to be created to store any includes you make, as a common place to put them.
        The dir can be created using the following cmd from the command prompt:
        mkdir %LOCALAPPDATA%\VoiceBot\ScriptExtension\
        The path created should be something similar the following:
        C:\Users\YOUR_USER_NAME_HERE\AppData\Local\VoiceBot\ScriptExtension\
        */
        public static string str_default_Include_path = (BFS.General.GetAppInstallPath() != "C:\\mnt\\W\\DevCore\\ProtoLab\\dotnet\\dotnet2k15\\Projects\\VoiceBotScript\\VoiceBotScript\\bin\\Debug") ?
        (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\VoiceBot\\ScriptExtension\\").Replace("\\", "\\\\")
        : "C:\\mnt\\DevCore\\ProtoLab\\dotnet\\dotnet2k15\\Projects\\VoiceBotScript\\VoiceBotScript\\ScriptExtension\\".Replace("\\", "\\\\");

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
        public static object Eval(IntPtr windowHandle, string str_code_block_name, string str_source_cs, string str_references = "")
          {
          object obj_return = null;
          str_references = (str_references.Length != 0 ? str_references : Includes.VoiceBotSupportClasses.Constants.str_default_references);
          //
          //CodeDomProvider provider = new CSharpCodeProvider(new Dictionary<String, String>{{ "CompilerVersion","v4.0" }});
          CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
          //
          CompilerParameters cp_compiler_params = new CompilerParameters();
          //>>>>>compileing options to build code
          cp_compiler_params.CompilerOptions = "/t:library";//<<--craft as library dll
          cp_compiler_params.GenerateExecutable = false;//<<--do not craft executable
          cp_compiler_params.GenerateInMemory = true;//<<--output in memmory vs output file
          cp_compiler_params.TreatWarningsAsErrors = false;//<<--warning handling
          str_references += (str_references.Length > 0 ? "|" : "") + "C:\\Program Files (x86)\\VoiceBot\\VoiceBot.exe";
          foreach( string str_reference in str_references.Split('|') )
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
          if( cr_result.Errors.Count > 0 )//>>>>>report error, and fail
            {
            foreach( CompilerError CompErr in cr_result.Errors )
              {
              string str_line = sb_script_core.ToString().Split('\n')[CompErr.Line - 1];
              //BFS.Dialog.ShowMessageError("Source:\n{SOURCE}".Replace("{SOURCE}",sb_script_core.ToString()) );
              BFS.Dialog.ShowMessageError("Eval\\\\ERROR: Source {NAME}".Replace("{NAME}", str_code_block_name));
              BFS.Dialog.ShowMessageError("Eval\\\\ERROR: Line number {LINE}, Error Number: {NUMBER}, '{TEXT}'".Replace("{LINE}", CompErr.Line.ToString()).Replace("{NUMBER}", CompErr.ErrorNumber).Replace("{TEXT}", CompErr.ErrorText));
              BFS.Dialog.ShowMessageError("Eval\\\\ERROR: Code on line number {NUMBER} => '{LINE}'".Replace("{NUMBER}", CompErr.Line.ToString()).Replace("{LINE}", str_line));
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
          return (obj_return);
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
          var obj_return = assembly_library_code.CreateInstance(str_fullname, bln_ignore_case, flags, null, obj_parameters_array, null, new object[] { });
          return (obj_return);
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
          return (obj_return);
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
          str_references = (str_references.Length != 0 ? str_references : Includes.VoiceBotSupportClasses.Constants.str_default_references);
          string str_code = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(BFS.ScriptSettings.ReadValue(str_include)));
          var obj_include = ScriptEngine.Eval(windowHandle, str_include, str_code, str_references);
          return ((object)obj_include);
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
        public static object LoadInclude(IntPtr windowHandle, string str_path, string str_references = "")
          {
          object obj_include = null;
          str_references = (str_references.Length != 0 ? str_references : Includes.VoiceBotSupportClasses.Constants.str_default_references);
          string str_include_path = (Includes.VoiceBotSupportClasses.Constants.str_default_Include_path + str_path + ".cs").Replace("\\\\", "\\");//<<--conditioning:: undo the script compatibility conditioning of the constant = app.path\ScriptExtension\IncludeFile.cs
                                                                                                                                                  //   
          try
            {
            TextReader tr_include_code = new StreamReader(str_include_path);
            string str_code = tr_include_code.ReadToEnd().ToString();
            tr_include_code.Close();
            //
            if( str_code.Length > 0 )
              { obj_include = ScriptEngine.Eval(windowHandle, str_include_path, str_code, str_references); }
            else
              {; }
            //</if(str_code.Length > 0)>
            }
          catch {; }
          //</try>
          return ((object)obj_include);
          }//</GetInclude(IntPtr windowHandle, string str_include,string str_references = "")>
           //-------------------------------------------------------------------
        }/*</class::ScriptEngine>*/
         //=====================================================================
         //== /ScriptEngine Function Library
         //=====================================================================
      #endregion
      //=====================================================================
      }/*</namespace::VoiceBotSupportClasses>*/
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
    /// ShipModel.
    /// </summary>
    public static class ShipModel
      {
      //>>>>>load includes, then wireup aliases to assembly
      static Type type_ShipModel = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_EliteDangerousShipModel", Includes.VoiceBotSupportClasses.Constants.str_default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.ShipModel")).ToArray<Type>()[0];
      //>>>>>appoint delagates (signatures)
      public delegate string GetStateDelegate(string str_property);
      public delegate void SetStateDelegate(string str_property, string str_value);
      public delegate void ResetShipStateDelegate();
      //>>>>>staff them
      public static GetStateDelegate GetState = (GetStateDelegate)Delegate.CreateDelegate(typeof(GetStateDelegate), ((type_ShipModel.GetMethods().Where(x => x.Name == "GetState").ToArray<MethodInfo>()))[0]);
      public static SetStateDelegate SetState = (SetStateDelegate)Delegate.CreateDelegate(typeof(SetStateDelegate), ((type_ShipModel.GetMethods().Where(x => x.Name == "SetState").ToArray<MethodInfo>()))[0]);
      public static ResetShipStateDelegate ResetShipState = (ResetShipStateDelegate)Delegate.CreateDelegate(typeof(ResetShipStateDelegate), ((type_ShipModel.GetMethods().Where(x => x.Name == "ResetShipState").ToArray<MethodInfo>()))[0]);
      }/*</class::ShipModel>*/
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
      public static Type type_FxLib = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_DarkLibs", Includes.VoiceBotSupportClasses.Constants.str_default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.FxLib")).ToArray<Type>()[0];
      //>>>>>appoint delagates (signatures)
      //public delegate Includes.VoiceBotSupportClasses.Args GetArgsDelegate(string str_variable_name);
      public delegate Dictionary<int, KeyValuePair<string, string>> GetArgsDelegate(string str_variable_name);
      public delegate void SaveArgsDelegate(string str_variable_name, string[] arr_content);
      //>>>>>staff them
      public static GetArgsDelegate GetArgs = (GetArgsDelegate)Delegate.CreateDelegate(typeof(GetArgsDelegate), ((type_FxLib.GetMethods().Where(x => x.Name == "GetArgs").ToArray<MethodInfo>()))[0]);
      public static SaveArgsDelegate SaveArgs = (SaveArgsDelegate)Delegate.CreateDelegate(typeof(SaveArgsDelegate), ((type_FxLib.GetMethods().Where(x => x.Name == "SaveArgs").ToArray<MethodInfo>()))[0]);
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
      static Type type_Speech = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_DarkLibs", Includes.VoiceBotSupportClasses.Constants.str_default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.Speech")).ToArray<Type>()[0];
      //>>>>>appoint delagates (signatures)
      public delegate string SpeechRecognizerDelegate(string str_voice_prompt);
      //>>>>>staff them
      public static SpeechRecognizerDelegate SpeechRecognizer = (SpeechRecognizerDelegate)Delegate.CreateDelegate(typeof(SpeechRecognizerDelegate), ((type_Speech.GetMethods().Where(x => x.Name == "SpeechRecognizer").ToArray<MethodInfo>()))[0]);
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
      static Type type_DBS = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_DarkLibs", Includes.VoiceBotSupportClasses.Constants.str_default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.DBS")).ToArray<Type>()[0];
      //>>>>>appoint delagates (signatures)
      public delegate DataTable QueryDelegate(string str_query, string str_connectionstring);
      //>>>>>staff them
      public static QueryDelegate Query = (QueryDelegate)Delegate.CreateDelegate(typeof(QueryDelegate), ((type_DBS.GetMethods().Where(x => x.Name == "Query").ToArray<MethodInfo>()))[0]);
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
  }
