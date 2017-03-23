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

//<references:>System.Core.dll |System.Data.dll | System.dll | System.Drawing.dll | System.Management.dll | System.Web.dll | System.Windows.Forms.dll | System.Xml.dll | mscorlib.dll | C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Profile\Client\System.Speech.dll | C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Profile\Client\System.Data.Linq.dll
//=============================================================================
/*
 * Disclaimer:
 * This file is both for show & tell, and for documentation: an example  VoiceBot
 * script lab environment.
 * 
 * Normally It is one class per file, however, as this is intended to not only be
 * functional, but to demonstrate voicebot sscript scaffolding and building using
 * the construct in the VisualStudio (2015) environment; so one file served the 
 * purpose.
 * Once in your build environment,
 * you can deconstuct the file into a proper project space one class per file, 
 * de-clutter of documentation comments, etc.
*/
//=============================================================================
namespace VoiceBotScriptTemplate
  {
  /*
   * The VS environment is built as a console app, and the base class is the
   * static class Program, with Main as the entry-point (typical)
   * The VoiceBotScript class,located in the region marked main, is intended to
   *  emulate the VoiceBot script space .Run(new IntPtr())  is the entry-point.
   * 
   * Once the script core is built, you should be able to copy, paste the region
   * marked main, into the VoiceBot script editor (copy paste the refeerences
   * string into the references input box, and make any "minor" adjustments for
   * environment differences, and the script should work just as it did int he
   * scaffold environment, with the benefit of the BFS calls doing their real
   * work, registry manipulation, sendkeys, the whole-lot.
   * 
   * An entire complext macro script chain can be built here and tested outside of the VoiceBot system, however a 
   * purchased VoiceBot install is required for references to work proper.
   */
  public static class Program
    {
    private static void Main(string[] args)
      {
      VoiceBotScript.Run(new IntPtr());
      }
    }


  #region Development Scaffolding Support Classes
  //=============================================================================
  //==Development Scaffolding Support Classes
  //=============================================================================
  /// <summary>
  /// developmental\\scaffolding stub for binary fortress services class while in 
  /// development (outside of voicebot editor> ie Visual Studio 
  /// (for intellisense and such purposes)
  /// (These should require any modification, however you are free to do so as
  /// desired.
  /// </summary>
  namespace BFS
    {
    //=========================================================================
    public static class Dialog
      {
      public static void ShowMessageError(string message)
        {/*<stub>*/
        Debug.WriteLine(message);
        }
      }

    //=========================================================================
    public static class General
      {
      public static String GetAppInstallPath()
        {/*<stub>*/
        var codeBase = Assembly.GetExecutingAssembly().CodeBase;
        var uri = new UriBuilder(codeBase);
        var path = Uri.UnescapeDataString(uri.Path);

        return Path.GetDirectoryName(path);
        }
      }

    //=========================================================================
    public static class Input
      {
      public static void SendKeys(string key_sequence)
        {/*<stub>*/
        Debug.WriteLine("SendKeys::{KEYS}".Replace("{KEYS}", key_sequence));
        }
      }

    //=========================================================================
    public static class Speech
      {
      public static void TextToSpeech(string message)
        {/*<stub>*/
        using( SpeechSynthesizer synth = new SpeechSynthesizer() )
          {
          //>>>>>Configure the audio output.
          synth.SetOutputToDefaultAudioDevice();
          //>>>>>Speak a string synchronously.
          synth.Speak(message);
          }
        }
      }

    //=========================================================================
    public class RegistryEntry : IEquatable<RegistryEntry>
      {
      public RegistryEntry(string key, string value)
        {
        Key = key;
        Dword = value;
        }

      public RegistryEntry()
        {; }

      public string Dword { get; set; }

      private string key;

      public string Key
        {
        get
          { return (key); }
        set
          {
          key = value;
          Id = new DateTime().Millisecond;
          }
        }

      private int Id { get; set; }

      public override string ToString()
        {
        return (Dword);
        }

      public override bool Equals(object obj)
        {
        var bln_return = false;

        if( obj != null )
          {
          var objAsRegistryEntry = obj as RegistryEntry;
          if( objAsRegistryEntry != null )
            { bln_return = Equals(objAsRegistryEntry); }
          else
            {; }
          }
        else
          {; }
        return (bln_return);
        }

      public override int GetHashCode()
        {
        return Id;
        }

      public bool Equals(RegistryEntry other)
        {
        var bln_return = false;
        if( other != null )
          { bln_return = (this.Key.Equals(other.Key)); }
        else
          {; }
        return (bln_return);
        }

      }

    //=========================================================================
    public static class ScriptSettings
      {
      private static List<RegistryEntry> lst_registry = new List<RegistryEntry>();

      public static string ReadValue(string variable_name)
        { /*<stub>*/
        var returnValue = "";
        var fnd = lst_registry.IndexOf(new RegistryEntry() { Key = variable_name });

        if( fnd != -1 )
          {
          returnValue = lst_registry[fnd].Dword;
          }
        else
          {
          returnValue = "";
          }

        Debug.WriteLine("**ReadValue(string {NAME})::{RETURN}".Replace("{NAME}", variable_name).Replace("{RETURN}", returnValue));
        return (returnValue);
        }

      //---------------------------------------------------------------------
      public static void WriteValue(string variable_name, string variable_value)
        {/*<stub>*/
        var fnd = lst_registry.IndexOf(new RegistryEntry() { Key = variable_name });
        //
        if( fnd == -1 )
          { lst_registry.Add(new RegistryEntry() { Key = variable_name, Dword = variable_value }); }
        else
          {
          if( variable_value.Length > 0 )
            { lst_registry[fnd].Dword = variable_value; }
          else
            { lst_registry.RemoveAt(fnd); }
          }
        Debug.WriteLine("**WriteValue(string {NAME}, string {VALUE})\\\\Fake Registry::".Replace("{NAME}", variable_name).Replace("{VALUE}", variable_value));
        foreach( RegistryEntry registry_entry in lst_registry )
          { Debug.WriteLine("***(Dword){KEY} = {DWORD}".Replace("{KEY}", registry_entry.Key).Replace("{DWORD}", registry_entry.Dword)); }
        }

      }
    }/*</namespace BFS>*/

  //=============================================================================
  //==Development Scaffolding Support Classes
  //=============================================================================

  #endregion Development Scaffolding Support Classes

  //*****************************************************************************
  /// <summary>
  /// Aural Operating System (AOS) Core script for VoiceBot
  /// </summary>
  //using System;
  //using System.Collections.Generic;//<<--needed for Dictionary
  //using System.Data; //<<--in references
  //using System.Data.OleDb; //<<--provided by System.Data.DataSetExtensions.dll
  //using System.Diagnostics;//<<--needed for debug (visual studio IDE only)
  //using System.Linq;//<<--provided by C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1\\Profile\\Client\\System.Data.Linq.dll
  //using System.Threading;
  //using System.Threading.Tasks;
  //using System.Drawing; //<<--in references
  //using System.IO;//<<--text file io for loading includes files
  //using System.Management; //<<--in references
  //using System.Speech.Recognition;//<<-- provided by C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1\\Profile\\Client\\System.Speech.dll
  //using System.Speech.Synthesis;
  //using System.Web; //<<--in references
  //using System.Windows; //<<--in references
  //using System.Xml; //<<--in references
  //using System.CodeDom.Compiler;
  //using System.Reflection;
  //using System.Text; //<<--provided by mscorlib.dll
  //using Microsoft.CSharp; //<<--in system.dll
  //<references:>System.Core.dll |System.Data.dll | System.dll | System.Drawing.dll | System.Management.dll | System.Web.dll | System.Windows.Forms.dll | System.Xml.dll | mscorlib.dll | C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Profile\Client\System.Speech.dll | C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\Profile\Client\System.Data.Linq.dll
  //=============================================================================

  #region main

  //=============================================================================
  //==main
  //=============================================================================
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
  //=============================================================================
  public static class VoiceBotScript
    {
    #region private vars

    private static string script_name = "AOSCore"; //<<--used to alias and have only one place to change when the name\\project changes

    #endregion private vars

    //---------------------------------------------------------------------------
    /// <summary>
    /// Author: Darkstrumn:\created::160107.13
    /// (
    /// This is a practical example, the reason I crafted this in the first place
    /// In this example, I wanted to build the ship AI verbal interface similar
    /// to what was getting done with Voice Attack, but that I wasn't seeing for
    /// VoiceBot which seemed to have the superior macro environment due impart
    /// to the use of C# as the scripting language, making the VoiceBot infinitely
    /// powerful for nearly anything.
    /// 
    /// I wanted afew core components that would allow the crafting of powerful and
    /// flexible macros with high code reusabilty through macro chaining and IPC
    /// via the BFS variable system.
    /// Access to db functionality is also available (in this example, an mdb file
    /// is used as a log book DB for trading, etc., but you could make anything.
    /// This following is the result:
    /// )
    /// Function: The Aural-OS Core is the workhorse of the macroscript system.
    ///           The system is typically 2 part, the caller, handles the interface
    ///           side of the system, doing random response, and such, then sends
    ///           the requested actions to the AOSCore script via IPC using variables
    ///           that are then executed from this script.
    ///           This systems allows the core logic to be in one script callable by
    ///           other scripts, easy to maintain, and add to.
    ///           While Random response can be handled by VoiceBot now, it can
    ///           also be specifically coded if the macro workflow demands it, or
    ///           that is your thing.
    /// <summary>
    /// <param name="windowHandle"></param>
    public static void Run(IntPtr windowHandle)
      {
      //<test case>
      //Includes.FxLib.SaveArgs("AOSCore_ipc", ("deployCargoScoop|SetShipState,cargoScoop,0|SendKeys,{HOME}|TTS,{TTS}".Replace("{TTS}",response)).Split('|') );//<<--second param is expected to be an array
      //Includes.FxLib.SaveArgs("AOSCore_ipc", ( @"TestMacro
      //|VCP`bln_confirm`Commander - Please confirm the jettisoning of all cargo.`Yes~Go a head~Please~Do it~Affirmative~Confirm~Confirmed~Correct~Continue~Accepted~Accept`Abort~No~Stop~Quit~Don't do it~Opps~Made a mistake~Belay that`Commander - I require verbal confirmation to proceed.~~If I may, a Yes or No would be sufficient confirmation sir.~~As I'm unable to understand your response - I will accept this as a failure to respond and abort. There may be a technical fault with the voice input system if you were indeed responding.
      //|Eval`TriggerLogic`if(BFS.ScriptSettings.ReadValue(""bln_confirm"") == ""true""){BFS.Input.SendKeys(""{DELETE}"");BFS.Speech.TextToSpeech(""Commander - All Cargo has been jettisoned, as ordered."");}else{BFS.Speech.TextToSpeech(""Commander - The jettisoning of cargo has been belayed, as ordered."");}`NULL".Replace("\r\n", "").Split('|') ));

      var fixture = @"TestMacro
            |VerbalConfirmationPrompt`bln_confirm`Commander - Please confirm the jettisoning of all cargo.`Yes~Go a head~Please~Do it~Affirmative~Confirm~Confirmed~Correct~Continue~Accepted~Accept`Abort~No~Stop~Quit~Don't do it~Opps~Made a mistake~Belay that`Commander - I require verbal confirmation to proceed.~~If I may, a Yes or No would be sufficient confirmation sir.~~As I'm unable to understand your response - I will accept this as a failure to respond and abort. There may be a technical fault with the voice input system if you were indeed responding.
            |Eval`TriggerLogic`_diagnostics = BFS.ScriptSettings.ReadValue(""bln_confirm"");if(BFS.ScriptSettings.ReadValue(""bln_confirm"") == ""true""){BFS.Input.SendKeys(""{LALT}"");BFS.Speech.TextToSpeech(""Commander - All Cargo has been jettisoned, as ordered."");}else{_bln_result = false;BFS.Speech.TextToSpeech(""Commander - The jettisoning of cargo has been belayed, as ordered."");}`NULL".Replace("\r\n", "");
      Includes.FxLib.SaveArgs("AOSCore_ipc", (fixture.Split('|')));

      var ipc_var_name = script_name + "_ipc";
      //>>>>>IPC\\Argument data can be passed forward from macro to macro using vars.
      var args = new Dictionary<int, KeyValuePair<string, string>>();
      try
        {
        if( ((Dictionary<int, KeyValuePair<string, string>>)Includes.FxLib.GetArgs(ipc_var_name)).Count > 0 )
          {
          args = (Dictionary<int, KeyValuePair<string, string>>)Includes.FxLib.GetArgs(ipc_var_name);
          BFS.Speech.TextToSpeech("The number of arguments found was " + args.Count.ToString());
          //foreach ( KeyValuePair<int, KeyValuePair<string, string>> arg in (Dictionary<int, KeyValuePair<string, string>>)args )
          //    {
          //    BFS.Speech.TextToSpeech("argument {KEY} is {VALUE}.".Replace("{KEY}", arg.Value.Key.ToString()).Replace("{VALUE}", arg.Value.Value.ToString()));
          //    }
          }
        else
          {; }
        }
      catch( Exception error )
        {
        BFS.Dialog.ShowMessageError(error.InnerException.Message);
        }

      //<diagnostics>try { if ( Includes.FxLib.GetArgs(ipc_var_name).Count > 0 ) { args = Includes.FxLib.GetArgs(ipc_var_name); } else {; }}catch(Exception error) {BFS.Dialog.ShowMessageError(" Includes.FxLib.GetArgs Includes-Error::{MESSAGE}".Replace("{MESSAGE}",error.Message));}
      var numParams = (args.Count > 2 ? (args.Count - 2) : 0); //<<--used for IPC argument handling
      var caller = (args.Count > 2 ? args[0].Value : ""); //<<--if called using IPC, this will be the calling module
                                                          //
      if( args.Count > 0 )//>>>>>we have args, parse and store
        {
        //>>>>>process based on IPC data
        for( var intLoop = 1 ; intLoop < (2 + numParams) ; intLoop++ )
          {
          var method = args[intLoop].Key;//<<--alias
                                         //var arrMethodArguments = args[intLoop].Value.Split('`');
                                         //var numberOfSubParams = (arrMethodArguments.Length);
                                         //IntPtr windowHandle
          CommandParser(windowHandle, args[intLoop].Key, args[intLoop].Value);
          }
        }
      else
        { /*BFS.Speech.TextToSpeech("No IPC data found.")*/; }

      if( args.Count != 0 )
        { Includes.FxLib.SaveArgs(ipc_var_name, null); }//<<--"delete" the value (I think the registry key remains...)
      }

    /*
     * The command parser is the core of the AOS function routing.
     * It uses a Function Dictionary to negate the need to use switch or if blocks
     * to navigate the available functions at the Developers command.
     * Note: all the diagnostic section can be removed once of no use
     */
    public static void CommandParser(IntPtr windowHandle, string method, string arguments)
      {
      var arrMethodArguments = arguments.Split('`');
      var numberOfSubParams = (arrMethodArguments.Length);
      //>>>>>diagnostics
      BFS.Speech.TextToSpeech("The number of sub-parameters found was " + numberOfSubParams.ToString());
      //>>>>>handle requested methods using function dictionary to refactor-out long switch statement, signatures made uniform for covenience
      var actions = new Dictionary<string, Delegate>();
      //>>>>>complex functions - heavy logic functions that can possibly be improved, but are the core of the AOS
      actions.Add("eval", new Func<IntPtr, string[], int, string>(Eval));
      actions.Add("verbalconfirmationprompt", new Func<IntPtr, string[], int, string>(VerbalConfirmationPrompt));
      actions.Add("verbalinputprompt", new Func<IntPtr, string[], int, string>(VerbalInputPrompt));
      actions.Add("verbalspellprompt", new Func<IntPtr, string[], int, string>(VerbalSpellPrompt));
      //>>>>>simple functions - lighter functions often in support of the complex functions
      actions.Add("texttospeech", new Func<IntPtr, string[], int, string>(TextToSpeech));
      actions.Add("initshipstate", new Func<IntPtr, string[], int, string>(InitShipState));
      actions.Add("getshipstate", new Func<IntPtr, string[], int, string>(GetShipState));
      actions.Add("setshipstate", new Func<IntPtr, string[], int, string>(SetShipState));
      actions.Add("setvalue", new Func<IntPtr, string[], int, string>(SetValue));
      actions.Add("sendkeys", new Func<IntPtr, string[], int, string>(SendKeys));

      if( actions.Keys.Contains(method.ToLower()) )
        {
        var action = actions[method.ToLower()];
        var result = action.DynamicInvoke(new object[] { windowHandle, arrMethodArguments, numberOfSubParams });
        }
      }

    /// <summary>
    /// when the construct is used with IPC subcommands, this is the arugments # error handler
    /// used mainly for dev (production grade code should not trigger this handler ;-P )
    /// </summary>
    /// <param name="method"></param>
    /// <param name="num_args"></param>
    /// <param name="numberOfExpectedSubParams"></param>
    private static void VoiceArgcError(IntPtr windowHandle, string method, int num_args, int numberOfExpectedSubParams)
      {
      /*Customize audio error as desired*/
      BFS.Speech.TextToSpeech(script_name + " Error detected in {METHOD}" + method + " parameters, number of arguments provided was {NUM}".Replace("{METHOD", method).Replace("NUM}", num_args.ToString()));
      BFS.Speech.TextToSpeech("Number of arguments expected is {NUM}".Replace("NUM}", numberOfExpectedSubParams.ToString()));
      }

    public static string Eval(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      /*
          |eval
              `JettisonAllCargoTriggerLogic
              `if(BFS.ScriptSettings.ReadValue("bln_confirm") == "true"){BFS.Input.SendKeys("{DELETE}");BFS.Speech.TextToSpeech("Commander - All Cargo has been jettisoned, as ordered.");}else{BFS.Speech.TextToSpeech("Commander - The jettisonning of cargo has been belayed, as ordered.");
              `<<NOTE:Here is where references go, if not using any, set to string.Empty:NOTE>>
      */
      var numberOfExpectedSubParams = 3;
      var returnValue = "ok";
      //>>>>>arrMethodArguments[0]; //<<--Descriptive name of code block
      //>>>>>arrMethodArguments[1]; //<<--code block
      //>>>>>arrMethodArguments[2]; //<<--references block <<NOTE:If not using any, set to string.Empty:NOTE>>
      if( numberOfSubParams == numberOfExpectedSubParams )//>>>>>execute
        {
        var codeBlockName = arrMethodArguments[0];
        var codeBlock = arrMethodArguments[1];
        var codeBlockReferences = arrMethodArguments[2].ToLower() != "null" ? arrMethodArguments[2] : "";
        Includes.VoiceBotSupportClasses.ScriptEngine.Eval(windowHandle, codeBlockName, codeBlock, codeBlockReferences);
        }
      else //>>>>>warn
        {
        VoiceArgcError(windowHandle, "EVAL", numberOfSubParams, numberOfExpectedSubParams);
        returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
        }
      return returnValue;
      }

    /*
    -Verbal Confirmation Prompt(VCP): used typically for simple confirmation prompting
    arrMethodArguments[0] //<<--storage variable for the VCP response, will be a boolean
    arrMethodArguments[1] //<<--Aural Confirmation Prompt verbiage
    arrMethodArguments[2] //<<--ValidTrue ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    arrMethodArguments[3] //<<--ValidFalse ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    arrMethodArguments[4] //<<--Retries ~ delimited string defining all retry responses to the user prompting them to try again using a response from the valid list of responses. use and empty response to skip retry response attempts, ie give a retry response every 2 retries... .
    Example use and parameter organization:
    Calling macro is called JettisonAllCargo, a non-toggle macro requiring confirmation of the action to proceed.
    An IPC message is setup to have the AOSCore process the VCP request and responding action
    "JettisonAllCargo
        |VCP
            `bln_confirm <<NOTE:the storage variable for the VCP response will be a boolean:NOTE>>
            `Commander - Please confirm the jettisoning of all cargo.
            `Yes~Go a head~Please~Do it~Affirmative~Confirm~Confirmed~Correct~Continue~Accepted~Accept
            `Abort~No~Stop~Quit~Don't do it~Opps~Made a mistake~Belay that
            <<NOTE: the Retries is a ~ delimited list of retry responses if the recognizer does not get "valid" responses.
            It will say them in order for each retry, to skip a retry response, ie say something for retry 1, skip 2, retry 3...
            use an empty response:Retries~retry 1~~retry 3... .:NOTE>>
            <<NOTE: if Retries is empty, the first answer will be accepted true or false, to allow for mistakes, at least 1 retry should be defined.:NOTE>>
            `Retries~Commander - I require verbal confirmation to proceed.~~If I may, a Yes or No would be sufficient confirmation sir.~~As I'm unable to understand your response - I will accept this as a failure to respond and abort. There may be a technical fault with the voice input system if you were indeed responding.
            }
        |Eval
            `if(BFS.ScriptSettings.ReadValue("bln_confirm") == "true"){BFS.Input.SendKeys("{DELETE}");BFS.Speech.TextToSpeech("Commander - All Cargo has been jettisoned, as ordered.");}else{BFS.Speech.TextToSpeech("Commander - The jettisoning of cargo has been belayed, as ordered.");}
    "
    */

    /// <summary>
    /// -Verbal Confirmation Prompt(VCP): used typically for aquiring simple verbal confirmation
    /// </summary>
    /// <param name="arrMethodArguments"></param>
    ///>>>>>arrMethodArguments[0] //<<--storage variable for the VCP response, will be a boolean
    ///>>>>>arrMethodArguments[1] //<<--Aural Confirmation Prompt verbiage
    ///>>>>>arrMethodArguments[2] //<<--ValidTrue ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    ///>>>>>arrMethodArguments[3] //<<--ValidFalse ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    ///>>>>>arrMethodArguments[4] //<<--Retries ~ delimited string defining all retry responses to the user prompting them to try again using a response from the valid list of responses. use and empty response to skip retry response attempts, ie give a retry response every 2 retries... .
    /// <param name="numberOfSubParams"></param>
    /// <param name="numberOfExpectedSubParams"></param>
    public static string VerbalConfirmationPrompt(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      var returnValue = "ok";
      var numberOfExpectedSubParams = 5;

      if( numberOfSubParams == numberOfExpectedSubParams )//>>>>>execute
        {
        var method = arrMethodArguments[1];
        var validTrue = arrMethodArguments[2].Split('~');
        var validFalse = arrMethodArguments[3].Split('~');
        var retryPrompts = arrMethodArguments[4].Split('~');
        //var user_response = "";
        var retryPrompt = arrMethodArguments[1];
        var quit = false;
        var validResponse = false;
        var confirmation = true;
        var retries = 0;
        var retries_left = 0;

        BFS.Speech.TextToSpeech(arrMethodArguments[1]);

        while( !validResponse && !quit )
          {
          var user_response = (string)Includes.Speech.SpeechRecognizer(retryPrompt);
          BFS.Speech.TextToSpeech("Commander, confirming that I heard: " + user_response);

          foreach( var valid_true in validTrue )
            {
            if( user_response.Trim().ToLower() != valid_true.Trim().ToLower() )
              { continue; }

            validResponse = true;
            confirmation = true;
            break;
            }

          if( validResponse )
            { continue; }

          foreach( var valid_false in validFalse )
            {
            if( user_response.Trim().ToLower() != valid_false.Trim().ToLower() )
              { continue; }

            validResponse = true;
            confirmation = false;
            break;
            }

          if( validResponse )
            { break; }
          retries++;//<<--increment failsafe value, this will allow the prompt to fail after so many retries
          retries_left = (retryPrompts.Length - retries);
          quit = (retries_left <= -1);
          //>>>>>see if we want to provide "help" to the user...
          retryPrompt = (!quit ? retryPrompts[retries - 1] : "");
          if( retryPrompt != string.Empty ) { BFS.Speech.TextToSpeech(retryPrompt); }
          }

        if( validResponse )
          {
          BFS.Speech.TextToSpeech("Commander, response accepted.");
          BFS.ScriptSettings.WriteValue(arrMethodArguments[0], (confirmation ? "true" : "false"));
          returnValue += ":" + confirmation.ToString();
          }
        else//>>>>>rejected
          { returnValue += ":" + confirmation.ToString(); }
        }
      else //>>>>>warn
        {
        VoiceArgcError(windowHandle, "", numberOfSubParams, numberOfExpectedSubParams);
        returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
        }
      return returnValue;
      }

    /*
    arrMethodArguments[0] //<<--storage variable for the VCP response, will be a boolean
    arrMethodArguments[1] //<<--Aural Input Prompt verbiage
    arrMethodArguments[2] //<<--ValidAccept ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    arrMethodArguments[3] //<<--ValidCancel ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    Example use and parameter organization:
    Calling macro is called NameShip, a non-toggle macro requiring verbal input of the ships name.
    An IPC message is setup to have the AOSCore process the VIP request and responding action
    "NameShip
        |VIP
            `ship_designation <<NOTE:the storage variable for the VCP response will be a boolean:NOTE>>
            `Commander - Please state ships new designation. Please finish with Accept to accept your verbal input and process.
            }
        |Eval
            `BFS.Speech.TextToSpeech("Commander - Ship name set to All Cargo has been jettisoned, as ordered.");}else{BFS.Speech.TextToSpeech("Commander - The jettisoning of cargo has been belayed, as ordered.");}
    "
    */

    /// <summary>
    /// Verbal Input Prompt(VIP): used typically for aquiring verbal dictation type responses
    /// </summary>
    /// <param name="arrMethodArguments"></param>
    ///>>>>>arrMethodArguments[0] //<<--storage variable for the VIP response, will be a boolean
    ///>>>>>arrMethodArguments[1] //<<--Aural Input Prompt verbiage
    ///>>>>>arrMethodArguments[2] //<<--ValidAccept ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    ///>>>>>arrMethodArguments[3] //<<--ValidCancel ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    /// <param name="numberOfSubParams"></param>
    /// <param name="numberOfExpectedSubParams"></param>
    public static string VerbalInputPrompt(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      var returnValue = "ok";
      var numberOfExpectedSubParams = 4;

      if( numberOfSubParams == numberOfExpectedSubParams )//>>>>>execute
        {
        var arr_valid_accept = "Accept~Submit~Confirm~Done".Split('~');
        var arr_valid_cancel = "Abort~Cancel".Split('~');
        var result = string.Empty;
        var validResponse = true;
        var quit = false;
        var bln_accepted = false;

        BFS.Speech.TextToSpeech(arrMethodArguments[0]);//<<--initial aural prompt

        while( !quit )
          {
          var user_response = (string)Includes.Speech.SpeechRecognizer("Ready");
          BFS.Speech.TextToSpeech("Commander, confirming that I heard: " + user_response);
          //>>>>>parse spoken data against list of valid accept tokens to see if content is complete (accepted)
          foreach( var valid_accept in arr_valid_accept )
            {
            if( user_response.Trim().ToLower() == valid_accept.Trim().ToLower() )
              {
              bln_accepted = true;
              break;
              }
            //>>>>>parse spoken data against list of valid cancel tokens to see if content is complete (cancelled)
            foreach( var valid_cancel in arr_valid_cancel )
              {
              if( user_response.Trim().ToLower() != valid_cancel.Trim().ToLower() )
                { continue; }
              validResponse = false;
              break;
              }
            //>>>>>if response is not valid, the input was accepted as a cancel token
            if( !validResponse )
              { quit = true; }
            else//>>>>>response is valid, response was either a command token to accept, or more input, look deeper...
              {
              if( !bln_accepted )//>>>>>if accepted is true, the input was the accept token, breech the loop
                { quit = true; }
              else//>>>>>append user input to the resulting speech to text buffer
                { result += (result.Length > 0 ? " " : "") + user_response; }
              }
            }
          //>>>>>if accepted, store value in VoiceBot variable supplied is arg0
          if( bln_accepted )
            {
            BFS.Speech.TextToSpeech("Commander, response accepted.");
            BFS.ScriptSettings.WriteValue(arrMethodArguments[0], result);
            returnValue += ":" + result;
            }
          else
            { returnValue = "cancelled"; }
          }
        }
      else //>>>>>warn
        {
        VoiceArgcError(windowHandle, "Verbal Input Prompt(VIP)", numberOfSubParams, numberOfExpectedSubParams);
        returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
        }
      return returnValue;
      }

    /*
    arrMethodArguments[0] //<<--storage variable for the VCP response, will be a boolean
    arrMethodArguments[1] //<<--Aural Input Prompt verbiage
    arrMethodArguments[2] //<<--ValidAccept ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    arrMethodArguments[3] //<<--ValidCancel ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    Example use and parameter organization:
    Calling macro is called NameShip, a non-toggle macro requiring verbal input of the ships name.
    An IPC message is setup to have the AOSCore process the VIP request and responding action
    "NameShip
        |VSP
            `system_name <<NOTE:the storage variable for the VCP response will be a boolean:NOTE>>
            `Commander - Please spell out the name of the system. Please finish with Accept to accept your verbal input and process.
            }
        |Eval
            `BFS.Speech.TextToSpeech("Commander - Ship name set to All Cargo has been jettisoned, as ordered.");}else{BFS.Speech.TextToSpeech("Commander - The jettisoning of cargo has been belayed, as ordered.");}
    "
    */

    /// <summary>
    /// Verbal Spell Prompt(VSP): used typically for prompts that may type the response into an input field, and requires greater accuracy on the spelling of the input (typically a single word)
    /// </summary>
    /// <param name="arrMethodArguments"></param>
    ///>>>>>arrMethodArguments[0] //<<--storage variable for the VSP response, will be a boolean
    ///>>>>>arrMethodArguments[1] //<<--Aural Input Prompt verbiage
    ///>>>>>arrMethodArguments[2] //<<--ValidAccept ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    ///>>>>>arrMethodArguments[3] //<<--ValidCancel ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
    /// <param name="numberOfSubParams"></param>
    /// <param name="numberOfExpectedSubParams"></param>
    public static string VerbalSpellPrompt(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      var returnValue = "ok";
      var numberOfExpectedSubParams = 4;
      //>>>>>validate
      if( numberOfSubParams == numberOfExpectedSubParams )//>>>>>execute
        {
        var arr_valid_accept = "Accept~Submit~Confirm~Done".Split('~');
        var arr_valid_cancel = "Abort~Cancel".Split('~');
        var user_response = "";
        var result = "";
        var validResponse = true;
        var quit = false;
        var bln_accepted = false;

        BFS.Speech.TextToSpeech(arrMethodArguments[0]);//<<--initial aural prompt

        while( !quit )
          {
          user_response = (string)Includes.Speech.SpeechRecognizer("Ready");
          BFS.Speech.TextToSpeech("Commander, confirming that I heard: " + user_response);
          //>>>>>parse spoken data against list of valid accept tokens to see if content is complete (accepted)
          foreach( var valid_accept in arr_valid_accept )
            {
            if( user_response.Trim().ToLower() == valid_accept.Trim().ToLower() )
              {
              bln_accepted = true;
              break;
              }
            //>>>>>parse spoken data against list of valid cancel tokens to see if content is complete (cancelled)
            foreach( var valid_cancel in arr_valid_cancel )
              {
              if( user_response.Trim().ToLower() != valid_cancel.Trim().ToLower() )
                { continue; }
              validResponse = false;
              break;
              }
            //>>>>>if response is not valid, the input was accepted as a cancel token
            if( !validResponse )
              {
              quit = true;
              }
            else//>>>>>response is valid, response was either a command token to accept, or more input, look deeper...
              {
              if( !bln_accepted )//>>>>>if accepted is true, the input was the accept token, breech the loop
                { quit = true; }
              else//>>>>>append user input to the resulting speech to text buffer
                { result += (result.Length > 0 ? " " : "") + user_response; }
              }
            }
          //>>>>>if accepted, store value in VoiceBot variable supplied is arg0
          if( bln_accepted )
            {
            BFS.Speech.TextToSpeech("Commander, response accepted.");
            BFS.ScriptSettings.WriteValue(arrMethodArguments[0], result);
            returnValue += ":" + result;
            }
          else
            { returnValue = "cancelled"; }
          }
        }
      else //>>>>>warn
        {
        VoiceArgcError(windowHandle, "Verbal Input Prompt(VIP)", numberOfSubParams, numberOfExpectedSubParams);
        returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
        }
      return returnValue;
      }

    public static string TextToSpeech(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      var method = arrMethodArguments[1];
      var numberOfExpectedSubParams = 1;
      var returnValue = "ok";
      //>>>>>arrMethodArguments[0]; //<<--message to speak
      if( numberOfSubParams == numberOfExpectedSubParams )//>>>>>execute
        {
        BFS.Speech.TextToSpeech(arrMethodArguments[0]);
        }
      else //>>>>>warn
        {
        VoiceArgcError(windowHandle, method.ToUpper(), numberOfSubParams, numberOfExpectedSubParams);
        returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
        }
      return returnValue;
      }

    public static string InitShipState(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      var returnValue = "ok";
      Includes.ShipModel.ResetShipState();
      return returnValue;
      }

    public static string GetShipState(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      var method = arrMethodArguments[1];
      var numberOfExpectedSubParams = 1;
      var returnValue = "ok";
      //>>>>>arrMethodArguments[0]; //<<--var to get
      if( numberOfSubParams == numberOfExpectedSubParams )//>>>>>execute
        {
        if( BFS.ScriptSettings.ReadValue("ShipState").Length < 1 ) { Includes.ShipModel.ResetShipState(); } else {; }//<<--init if not present
        returnValue = Includes.ShipModel.GetState(arrMethodArguments[0]);
        }
      else //>>>>>warn
        {
        VoiceArgcError(windowHandle, method.ToUpper(), numberOfSubParams, numberOfExpectedSubParams);
        returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
        }
      return returnValue;
      }

    public static string SetShipState(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      var method = arrMethodArguments[1];
      var numberOfExpectedSubParams = 2;
      var returnValue = "ok";
      //>>>>>arrMethodArguments[0]; //<<--var to set
      //>>>>>arrMethodArguments[1]; //<<--value to set var to
      if( numberOfSubParams == numberOfExpectedSubParams )//>>>>>execute
        {
        if( BFS.ScriptSettings.ReadValue("ShipState").Length < 1 ) { Includes.ShipModel.ResetShipState(); } else {; }//<<--init if not present
        Includes.ShipModel.SetState(arrMethodArguments[0], arrMethodArguments[1]);
        }
      else //>>>>>warn
        {
        VoiceArgcError(windowHandle, method.ToUpper(), numberOfSubParams, numberOfExpectedSubParams);
        returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
        }
      return returnValue;
      }

    public static string SetValue(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      var method = arrMethodArguments[1];
      var numberOfExpectedSubParams = 2;
      var returnValue = "ok";
      //>>>>>arrMethodArguments[0]; //<<--var to set
      //>>>>>arrMethodArguments[1]; //<<--value to set var to
      if( numberOfSubParams == numberOfExpectedSubParams )//>>>>>execute
        {
        Includes.FxLib.SaveArgs(arrMethodArguments[0], arrMethodArguments[1].Split('|'));
        }
      else //>>>>>warn
        {
        VoiceArgcError(windowHandle, method.ToUpper(), numberOfSubParams, numberOfExpectedSubParams);
        returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
        }
      return returnValue;
      }

    public static string SendKeys(IntPtr windowHandle, string[] arrMethodArguments, int numberOfSubParams)
      {
      var method = arrMethodArguments[1];
      var numberOfExpectedSubParams = 1;
      var returnValue = "ok";
      //>>>>>arrMethodArguments[0]; //<<--key-sequence to send
      if( numberOfSubParams == numberOfExpectedSubParams )//>>>>>execute
        {
        BFS.Input.SendKeys(arrMethodArguments[0]);
        }
      else //>>>>>warn
        {
        VoiceArgcError(windowHandle, method.ToUpper(), numberOfSubParams, numberOfExpectedSubParams);
        returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
        }
      return returnValue;
      }

    }

  //=============================================================================
  //==/main
  //=============================================================================

  #endregion main

  //=============================================================================

  #region Includes

  //=============================================================================
  //== Includes Classes
  //=============================================================================
  /// <summary>
  /// This part is part of the scaffold, only the bits in the main region are for
  /// the VoiceBot script editor when copy-pasting.
  /// 
  /// Note: The Constants.default_references may also need to be copy-pasted into the
  /// references input box in the VoiceBot Script Editor.
  /// 
  /// Includes
  /// How it works: includes are accomplished using the ScriptEngine coded below
  /// in the region marked "Support Classes" to in-line compile the "include" code
  /// into an assembly in memory. The in-memory assembly is then accessed directly
  /// or via delegates that connect the includes code to the Includes namespace.
  /// calls can then be made fully qualifying the call ie:
  /// string confirmation_response = Includes.Speech.SpeechRecognizer("Are your sure?");
  /// or directly:
  /// var assem_Speech = (Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(),"Include_DarkLibs",Includes.VoiceBotSupportClasses.Constants.default_references)).GetTypes().Where( x => x.FullName.Contains("Includes.Speech");
  /// string confirmation_response = (string)CallFunction(assem_Speech, "SpeechRecognizer", new object[] {"Are your sure?"})
  /// -----------------------------------------------------------------------
  /// Usings: using System.IO;//<<--text file io for loading includes files
  /// References: N/A
  /// NOTE: Includes can be done in 2 ways, using a separate .cs file to hold the
  /// cs code loaded via LoadIncludes(), and\or the VoiceBot registry variables loaded
  /// via GetIncludes(). The latter is more portable, easier to edit and backup,
  /// however, the former, is more compact and self-contained being stored in the
  /// registry, but must be exported from the registry to backup, and or share.
  /// The choice is yours on which to use.
  /// There are 2 example include files Includ_DarkLibs the Script Engine is the only
  /// bit that was duplicated in the include for the include, as the internal scaffolding
  /// ScriptEngine is how the script code even gets called, so it was a neccessary evil.
  /// in this file, but were made into a proof of concept to see if includes could
  /// be made. Yes, they can!
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
        //private static string assemplyPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
        private static string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

        public static string default_references = "System.dll | System.Core.dll |System.Data.dll | System.Drawing.dll | System.Management.dll | System.Web.dll | System.Windows.Forms.dll | System.Xml.dll | mscorlib.dll |  C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\Microsoft.CSharp.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Speech.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Data.Linq.dll".Replace("\\", "\\\\");

        public static string default_connection_string = "Provider='Microsoft.ACE.OLEDB.12.0'; Data Source='{PATH}LogBook.mdb'; Persist Security Info=False".Replace("{PATH}", (BFS.General.GetAppInstallPath() + "\\ScriptExtension\\")).Replace("\\", "\\\\");
        /*
        The following path will likely need to be created to store any includes you make, as a common place to put them.
        The dir can be created using the following cmd from the command prompt:
        mkdir %LOCALAPPDATA%\VoiceBot\ScriptExtension\
        The path created should be something similar the following:
        C:\Users\YOUR_USER_NAME_HERE\AppData\Local\VoiceBot\ScriptExtension\
        */

        public static string default_Include_path = (appPath + "\\ScriptExtension\\").Replace("\\", "\\\\");
        }

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
        public static object Eval(IntPtr windowHandle, string codeBlockName, string source_cs, string references = "")
          {
          StringBuilder sb_script_core = new StringBuilder("");
          object[] obj_parameters_array;
          var class_name = "TriggerLogic";
          var function_name = "{CLASS}.Run".Replace("{CLASS}", class_name);
          var code = "";
          var code_template = @"
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
      private string _error = ""ok"";
      private string _diagnostics = """";
      public TriggerLogic()
          {;}
      public bool Result{get{return(_bln_result);}}
      public string LastError{get{return(_error);}}
      public string DiagnosticsMessage{get{return(_diagnostics);}}
	    public bool Run(IntPtr windowHandle)
	    {
        try
            {
		    {CODEBLOCK}
            }
        catch(Exception error)
            {
            _error = ""TriggerLogic: fail - {ERROR}."".Replace(""{ERROR}"", error.Message);
            }

        return(Result);
	    }
    }
}";
          /*
          execute VHP's (variable hardpoints), allows us to add token handling to the code builder, can be a one-liner,
          but is clearer broken down. Note: order is important.
          */
          code = code_template.Replace("{CODEBLOCK}", source_cs);
          code = code.Replace("{CODEBLOCKNAME}", codeBlockName);
          code = code.Replace("{REFERENCES}", Includes.VoiceBotSupportClasses.Constants.default_references);
          sb_script_core.Append(code);
          var assembly_trigger_logic = (Assembly)ScriptEngine.CsCodeAssembler(windowHandle, codeBlockName, sb_script_core.ToString(), references = "");
          obj_parameters_array = new object[] { };
          dynamic obj_class = CreateClassInstance(assembly_trigger_logic, class_name, obj_parameters_array);
          object obj_return = obj_class.Run(windowHandle);

          return (obj_return);
          }

        /// <summary>
        /// Author: Darkstrumn
        /// Function: CsCodeAssembler takes provided CS source and attempts to compile code, then returns an instance object of the assembly
        /// viable for loading aux scripts to form includes or dynamic code loading
        /// </summary>
        /// <param name="windowHandle"></param>
        /// <param name="source_cs"></param>
        /// <param name="references"></param>
        /// <returns></returns>
        public static Assembly CsCodeAssembler(IntPtr windowHandle, string codeBlockName, string source_cs, string references = "")
          {
          Assembly obj_return = null;
          references = (references.Length != 0 ? references : Includes.VoiceBotSupportClasses.Constants.default_references);
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
          cp_compiler_params.IncludeDebugInformation = true;
          references += (references.Length > 0 ? "|" : "") + "C:\\Program Files (x86)\\VoiceBot\\VoiceBot.exe";//<<--add BFS library to the references
          foreach( string reference in references.Split('|') )
            {
            System.Diagnostics.Debug.WriteLine("Adding reference: " + reference.Trim());
            cp_compiler_params.ReferencedAssemblies.Add(reference.Trim());
            }

          StringBuilder sb_script_core = new StringBuilder("");//<<--more flexible than string concatination if we add hardcoded library logic here

          sb_script_core.Append(source_cs);//</sb_script_core>

          CompilerResults cr_result = provider.CompileAssemblyFromSource(cp_compiler_params, sb_script_core.ToString());

          if( cr_result.Errors.Count > 0 )//>>>>>report error, and fail
            {
            foreach( CompilerError CompErr in cr_result.Errors )
              {
              string line = sb_script_core.ToString().Split('\n')[CompErr.Line - (CompErr.Line > 0 ? 1 : 0)];
              //BFS.Dialog.ShowMessageError("Source:\n{SOURCE}".Replace("{SOURCE}",sb_script_core.ToString()) );
              BFS.Dialog.ShowMessageError("CsCodeAssembler\\\\ERROR: Source {NAME}".Replace("{NAME}", codeBlockName));
              BFS.Dialog.ShowMessageError("CsCodeAssembler\\\\ERROR: Line number {LINE}, Error Number: {NUMBER}, '{TEXT}'".Replace("{LINE}", CompErr.Line.ToString()).Replace("{NUMBER}", CompErr.ErrorNumber).Replace("{TEXT}", CompErr.ErrorText));
              BFS.Dialog.ShowMessageError("CsCodeAssembler\\\\ERROR: Code on line number {NUMBER} => '{LINE}'".Replace("{NUMBER}", CompErr.Line.ToString()).Replace("{LINE}", line));
              }
            }
          else//>>>>>execute code
            {
            System.Reflection.Assembly assembly_library_code = cr_result.CompiledAssembly;

            obj_return = assembly_library_code;
            }

          return (obj_return);
          }

        /// <summary>
        /// call non-statics like the type class Args
        /// </summary>
        /// <param name="assembly_library_code"></param>
        /// <param name="class_name"></param>
        /// <param name="function_name"></param>
        /// <param name="obj_parameters_array"></param>
        /// <returns></returns>
        public static dynamic CreateClassInstance(Assembly assembly_library_code, string class_name, object[] obj_parameters_array, System.Reflection.BindingFlags flags = (BindingFlags.Public | BindingFlags.Instance))
          {
          //<diagnostics to see if member names are proper>((((System.Reflection.RuntimeAssembly)assembly_library_code).DefinedTypes).Where(c=>c.FullName.Contains("Includes." + class_name)))
          var fullname = "Includes." + class_name;
          var bln_ignore_case = false;
          //var flags = (BindingFlags.Public | BindingFlags.Instance);
          var returnValue = assembly_library_code.CreateInstance(fullname, bln_ignore_case, flags, null, obj_parameters_array, null, new object[] { });
          return (returnValue);
          }

        /// <summary>
        /// Author: Darkstrumn
        /// Function:  CallFunction call the included functions specified
        /// </summary>
        /// <param name="obj_library_code_instance"></param>
        /// <param name="function_name"></param>
        /// <param name="obj_parameters_array"></param>
        /// <returns></returns>
        public static object CallFunction(Assembly assembly_library_code, string function_name, object[] obj_parameters_array)
          {
          var obj_library_code_instance = assembly_library_code.CreateInstance("Includes." + function_name);
          //<diagnostics>Type[] obj_library_types = ((Assembly)assembly_library_code).GetTypes();
          var type_instance_type = obj_library_code_instance.GetType();
          var method_info = type_instance_type.GetMethod(function_name);

          object obj_return = method_info.Invoke(obj_library_code_instance, obj_parameters_array);

          return (obj_return);
          }

        /// <summary>
        /// Author: Darkstrumn
        /// Function: GetInclude is an alias to the loading of Includes. Store unboxed object to instance variable and use it with the CallFunction
        /// function to call the included functions
        /// </summary>
        /// <param name="windowHandle"></param>
        /// <param name="include"></param>
        /// <param name="references"></param>
        /// <returns></returns>
        public static object GetInclude(IntPtr windowHandle, string include, string references = "")
          {
          references = (references.Length != 0 ? references : Includes.VoiceBotSupportClasses.Constants.default_references);
          var code = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(BFS.ScriptSettings.ReadValue(include)));
          var obj_include = ScriptEngine.CsCodeAssembler(windowHandle, include, code, references);
          return ((object)obj_include);
          }

        /// <summary>
        /// Author: Darkstrumn
        /// Function: encode - Intially intended to emulate the way VoiceBot stores it macroscripts this also has the effect of
        /// shrinking and preserving it textual content as well as making it easy to transport via the web
        /// large amounts of data. Thus it can be uses as poor-man's compression, so I refactored it out here
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Base64Encode(string content)
          {
          var returnValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content));
          return (returnValue);
          }

        /// <summary>
        /// Author: Darkstrumn
        /// Function: decodaaIntially intended to emulate the way VoiceBot stores it macroscripts this also has the effect of
        /// shrinking and preserving its textual content as well as making it easy to transport via the web
        /// large amounts of data. Thus it can be uses as poor-man's compression, so I refactored it out here
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Base64Decode(string content)
          {
          var returnValue = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(BFS.ScriptSettings.ReadValue(content)));
          return (returnValue);
          }

        /// <summary>
        /// Author: Darkstrumn
        /// Function: LoadInclude is an alias to the loading of includes via local cs file. Store unboxed object to instance variable and use it with the CallFunction
        /// function to call the included functions
        /// </summary>
        /// <param name="windowHandle"></param>
        /// <param name="path"></param>
        /// <param name="references"></param>
        /// <returns></returns>
        public static object LoadInclude(IntPtr windowHandle, string path, string references = "")
          {
          object obj_include = null;
          references = (references.Length != 0 ? references : Includes.VoiceBotSupportClasses.Constants.default_references);
          var include_path = (Path.Combine(Includes.VoiceBotSupportClasses.Constants.default_Include_path, path + ".cs")).Replace("\\\\", "\\").Replace("file:\\", "");//<<--conditioning:: undo the script compatibility conditioning of the constant = app.path\ScriptExtension\IncludeFile.cs

          try
            {
            TextReader tr_include_code = new StreamReader(include_path);
            string code = tr_include_code.ReadToEnd().ToString();
            tr_include_code.Close();

            if( code.Length > 0 )
              { obj_include = ScriptEngine.CsCodeAssembler(windowHandle, include_path.Split('\\')[include_path.Split('\\').Length - 1].Replace(".cs", ""), code, references); }
            }
          catch( Exception error )
            {
            System.Diagnostics.Debug.WriteLine(error.Message);
            }

          return ((object)obj_include);
          }
        }

      //=====================================================================
      //== /ScriptEngine Function Library
      //=====================================================================

      #endregion ScriptEngine

      //=====================================================================
      }

    //=========================================================================
    //== /Support Classes
    //=========================================================================

    #endregion Support Classes

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
      private static Type type_ShipModel = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_EliteDangerousShipModel", Includes.VoiceBotSupportClasses.Constants.default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.ShipModel")).ToArray<Type>()[0];

      //>>>>>appoint delagates (signatures)
      public delegate string GetStateDelegate(string property);

      public delegate void SetStateDelegate(string property, string value);

      public delegate void ResetShipStateDelegate();

      //>>>>>staff them
      public static GetStateDelegate GetState = (GetStateDelegate)Delegate.CreateDelegate(typeof(GetStateDelegate), ((type_ShipModel.GetMethods().Where(x => x.Name == "GetState").ToArray<MethodInfo>()))[0]);

      public static SetStateDelegate SetState = (SetStateDelegate)Delegate.CreateDelegate(typeof(SetStateDelegate), ((type_ShipModel.GetMethods().Where(x => x.Name == "SetState").ToArray<MethodInfo>()))[0]);
      public static ResetShipStateDelegate ResetShipState = (ResetShipStateDelegate)Delegate.CreateDelegate(typeof(ResetShipStateDelegate), ((type_ShipModel.GetMethods().Where(x => x.Name == "ResetShipState").ToArray<MethodInfo>()))[0]);
      }

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
      public static Assembly assem_FxLib = (Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_DarkLibs", Includes.VoiceBotSupportClasses.Constants.default_references);
      public static Type type_FxLib = assem_FxLib.GetTypes().Where(x => x.FullName.Contains("Includes.FxLib")).ToArray<Type>()[0];
      public delegate Dictionary<int, KeyValuePair<string, string>> GetArgsDelegate(string variable_name);

      public delegate void SaveArgsDelegate(string variable_name, string[] arr_content);

      //>>>>>staff them
      public static GetArgsDelegate GetArgs = (GetArgsDelegate)Delegate.CreateDelegate(typeof(GetArgsDelegate), (((type_FxLib.GetMethods().Where(x => x.Name == "GetArgs")).ToArray<MethodInfo>()))[0]);

      public static SaveArgsDelegate SaveArgs = (SaveArgsDelegate)Delegate.CreateDelegate(typeof(SaveArgsDelegate), (((type_FxLib.GetMethods().Where(x => x.Name == "SaveArgs")).ToArray<MethodInfo>()))[0]);
      }

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
      private static Type type_Speech = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_DarkLibs", Includes.VoiceBotSupportClasses.Constants.default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.Speech")).ToArray<Type>()[0];

      //>>>>>appoint delagates (signatures)
      public delegate string SpeechRecognizerDelegate(string voice_prompt);

      //>>>>>staff them
      public static SpeechRecognizerDelegate SpeechRecognizer = (SpeechRecognizerDelegate)Delegate.CreateDelegate(typeof(SpeechRecognizerDelegate), ((type_Speech.GetMethods().Where(x => x.Name == "SpeechRecognizer").ToArray<MethodInfo>()))[0]);
      }

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
      private static Type type_DBS = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_DarkLibs", Includes.VoiceBotSupportClasses.Constants.default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.DBS")).ToArray<Type>()[0];

      //>>>>>appoint delagates (signatures)
      public delegate DataTable QueryDelegate(string query, string connectionstring);

      //>>>>>staff them
      public static QueryDelegate Query = (QueryDelegate)Delegate.CreateDelegate(typeof(QueryDelegate), ((type_DBS.GetMethods().Where(x => x.Name == "Query").ToArray<MethodInfo>()))[0]);
      }

    //=========================================================================
    //== /Support Classes
    //=========================================================================
    }

  //=============================================================================
  //== /Includes Classes
  //=============================================================================

  #endregion Includes

  //=============================================================================
  }