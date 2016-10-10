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
 * Normally It is one class per file, however, as this is intended to not only be
 * functional, but to demonstrate voicebot sscript scaffolding and building using
 * the construct, so one file served the purpose. Once in your build environment,
 * you can deconstuct the file into a proper project space one class per file, etc.
*/
//=============================================================================
namespace VoiceBotScriptTemplate
{
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
    /// developmental\\scaffolding stub for binary fortress services class while in development (outside of voicebot editor> ie visual studio (for intellisense and such purposes)
    /// </summary>
    namespace BFS
    {
        //=========================================================================
        public static class Dialog
        {
            public static void ShowMessageError(string str_message)
            {/*<stub>*/
                Debug.WriteLine(str_message);
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
            public static void SendKeys(string str_key_sequence)
            {/*<stub>*/
                Debug.WriteLine("SendKeys::{KEYS}".Replace("{KEYS}", str_key_sequence));
            }
        }

        //=========================================================================
        public static class Speech
        {
            public static void TextToSpeech(string str_message)
            {/*<stub>*/
                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                {
                    //>>>>>Configure the audio output.
                    synth.SetOutputToDefaultAudioDevice();
                    //>>>>>Speak a string synchronously.
                    synth.Speak(str_message);
                }
            }
        }

        //=========================================================================
        public class RegistryEntry : IEquatable<RegistryEntry>
        {
            public RegistryEntry(string str_key, string str_value)
            {
                Key = str_key;
                Dword = str_value;
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

                if (obj != null)
                {
                    var objAsRegistryEntry = obj as RegistryEntry;
                    if (objAsRegistryEntry != null)
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
                if (other != null)
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

            public static string ReadValue(string str_variable_name)
            { /*<stub>*/
                var str_return = "";
                var fnd = lst_registry.IndexOf(new RegistryEntry() { Key = str_variable_name });

                if (fnd != -1)
                {
                    str_return = lst_registry[fnd].Dword;
                }
                else
                {
                    str_return = "";
                }

                Debug.WriteLine("**ReadValue(string {NAME})::{RETURN}".Replace("{NAME}", str_variable_name).Replace("{RETURN}", str_return));
                return (str_return);
            }

            //---------------------------------------------------------------------
            public static void WriteValue(string str_variable_name, string str_variable_value)
            {/*<stub>*/
                var fnd = lst_registry.IndexOf(new RegistryEntry() { Key = str_variable_name });
                //
                if (fnd == -1)
                { lst_registry.Add(new RegistryEntry() { Key = str_variable_name, Dword = str_variable_value }); }
                else
                {
                    if (str_variable_value.Length > 0)
                    { lst_registry[fnd].Dword = str_variable_value; }
                    else
                    { lst_registry.RemoveAt(fnd); }
                }
                Debug.WriteLine("**WriteValue(string {NAME}, string {VALUE})\\\\Fake Registry::".Replace("{NAME}", str_variable_name).Replace("{VALUE}", str_variable_value));
                foreach (RegistryEntry registry_entry in lst_registry)
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

        private static string str_script_name = "AOSCore";

        #endregion private vars

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
            //<test case>
            //Includes.FxLib.SaveArgs("AOSCore_ipc", ("deployCargoScoop|SetShipState,cargoScoop,0|SendKeys,{HOME}|TTS,{TTS}".Replace("{TTS}",str_response)).Split('|') );//<<--second param is expected to be an array
            //Includes.FxLib.SaveArgs("AOSCore_ipc", ( @"TestMacro
            //|VCP`bln_confirm`Commander - Please confirm the jettisoning of all cargo.`Yes~Go a head~Please~Do it~Affirmative~Confirm~Confirmed~Correct~Continue~Accepted~Accept`Abort~No~Stop~Quit~Don't do it~Opps~Made a mistake~Belay that`Commander - I require verbal confirmation to proceed.~~If I may, a Yes or No would be sufficient confirmation sir.~~As I'm unable to understand your response - I will accept this as a failure to respond and abort. There may be a technical fault with the voice input system if you were indeed responding.
            //|Eval`TriggerLogic`if(BFS.ScriptSettings.ReadValue(""bln_confirm"") == ""true""){BFS.Input.SendKeys(""{DELETE}"");BFS.Speech.TextToSpeech(""Commander - All Cargo has been jettisoned, as ordered."");}else{BFS.Speech.TextToSpeech(""Commander - The jettisoning of cargo has been belayed, as ordered."");}`NULL".Replace("\r\n", "").Split('|') ));

            var str_fixture = @"TestMacro
            |VCP`bln_confirm`Commander - Please confirm the jettisoning of all cargo.`Yes~Go a head~Please~Do it~Affirmative~Confirm~Confirmed~Correct~Continue~Accepted~Accept`Abort~No~Stop~Quit~Don't do it~Opps~Made a mistake~Belay that`Commander - I require verbal confirmation to proceed.~~If I may, a Yes or No would be sufficient confirmation sir.~~As I'm unable to understand your response - I will accept this as a failure to respond and abort. There may be a technical fault with the voice input system if you were indeed responding.
            |Eval`TriggerLogic`if(BFS.ScriptSettings.ReadValue(""bln_confirm"") == ""true""){BFS.Input.SendKeys(""{LALT}"");BFS.Speech.TextToSpeech(""Commander - All Cargo has been jettisoned, as ordered."");}else{_bln_result = false;BFS.Speech.TextToSpeech(""Commander - The jettisoning of cargo has been belayed, as ordered."");}`NULL".Replace("\r\n", "");
            Includes.FxLib.SaveArgs("AOSCore_ipc", (str_fixture.Split('|')));

            var ipc_var_name = str_script_name + "_ipc";
            //>>>>>IPC\\Argument data can be passed forward from macro to macro using vars.
            var args = new Dictionary<int, KeyValuePair<string, string>>();
            try
            {
                if (((Dictionary<int, KeyValuePair<string, string>>)Includes.FxLib.GetArgs(ipc_var_name)).Count > 0)
                {
                    args = (Dictionary<int, KeyValuePair<string, string>>)Includes.FxLib.GetArgs(ipc_var_name);
                    BFS.Speech.TextToSpeech("The number of arguments found was " + args.Count.ToString());
                    //foreach ( KeyValuePair<int, KeyValuePair<string, string>> arg in (Dictionary<int, KeyValuePair<string, string>>)args )
                    //    {
                    //    BFS.Speech.TextToSpeech("argument {KEY} is {VALUE}.".Replace("{KEY}", arg.Value.Key.ToString()).Replace("{VALUE}", arg.Value.Value.ToString()));
                    //    }
                }
                else {; }
                //</if ( Includes.FxLib.GetArgs(ipc_var_name).Count > 0 )>
            }
            catch (Exception error)
            {
                BFS.Dialog.ShowMessageError(error.InnerException.Message);
            }
            //<diagnostics>try { if ( Includes.FxLib.GetArgs(ipc_var_name).Count > 0 ) { args = Includes.FxLib.GetArgs(ipc_var_name); } else {; }}catch(Exception error) {BFS.Dialog.ShowMessageError(" Includes.FxLib.GetArgs Includes-Error::{MESSAGE}".Replace("{MESSAGE}",error.Message));}
            var int_numParams = (args.Count > 2 ? (args.Count - 2) : 0); //<<--used for IPC argument handling
            var str_caller = (args.Count > 2 ? args[0].Value : ""); //<<--if called using IPC, this will be the calling module
                                                                    //
            if (args.Count > 0)//>>>>>we have args, parse and store
            {
                //>>>>>process based on IPC data
                for (var intLoop = 1; intLoop < (2 + int_numParams); intLoop++)
                {
                    var str_method = args[intLoop].Key;//<<--alias
                    //var arr_arguments = args[intLoop].Value.Split('`');
                    //var int_num_subparams = (arr_arguments.Length);
                    //IntPtr windowHandle
                    CommandParser(windowHandle, args[intLoop].Key, args[intLoop].Value);
                }//</for(var intLoop = 0; intLoop < args.Count; intLoop++)>
            }
            else
            { /*BFS.Speech.TextToSpeech("No IPC data found.")*/; }
            //</if(int_argc > 0)>
            //
            if (args.Count != 0)
            { Includes.FxLib.SaveArgs(ipc_var_name, null); }//<<--"delete" the value (I think the registry key remains...)
            else
            {; }
            //</if ( args.Count != 0 )>
        }/*</Run(IntPtr windowHandle)>*/

        public static void CommandParser(IntPtr windowHandle, string str_method, string str_arguments)
        {
            var arr_arguments = str_arguments.Split('`');
            var int_num_subparams = (arr_arguments.Length);
            //>>>>>diagnostics
            BFS.Speech.TextToSpeech("The number of sub-parameters found was " + int_num_subparams.ToString());
            //>>>>>handle requested methods
            var actions = new Dictionary<string, Delegate>();

            actions.Add("eval", new Func<IntPtr, string[], int, string>(Eval));
            actions.Add("VerbalConfirmationPrompt", new Func<IntPtr, string[], int, string>(VerbalConfirmationPrompt));
            actions.Add("VerbalInputPrompt", new Func<IntPtr, string[], int, string>(VerbalInputPrompt));
            actions.Add("VerbalSpellPrompt", new Func<IntPtr, string[], int, string>(VerbalSpellPrompt));

            actions.Add("texttospeech", new Func<IntPtr, string[], int, string>(TextToSpeech));
            actions.Add("initshipstate", new Func<IntPtr, string[], int, string>(InitShipState));
            actions.Add("getshipstate", new Func<IntPtr, string[], int, string>(GetShipState));
            actions.Add("setshipstate", new Func<IntPtr, string[], int, string>(SetShipState));
            actions.Add("setvalue", new Func<IntPtr, string[], int, string>(SetValue));
            actions.Add("sendkeys", new Func<IntPtr, string[], int, string>(SendKeys));

            var action = actions.Where(a => a.Key.ToLower() == str_method.ToLower()).Select(act => act.Value.DynamicInvoke(new { windowHandle, arr_arguments, int_num_subparams }));
        }

        //-------------------------------------------------------------------------
        /// <summary>
        /// when the construct is used with IPC subcommands, this is the arugments # error handler
        /// </summary>
        /// <param name="str_method"></param>
        /// <param name="int_num_args"></param>
        /// <param name="int_expected_num"></param>
        private static void VoiceArgcError(IntPtr windowHandle, string str_method, int int_num_args, int int_expected_num)
        {
            /*Customize audio error as desired*/
            BFS.Speech.TextToSpeech(str_script_name + " Error detected in {METHOD}" + str_method + " parameters, number of arguments provided was {NUM}".Replace("{METHOD", str_method).Replace("NUM}", int_num_args.ToString()));
            BFS.Speech.TextToSpeech("Number of arguments expected is {NUM}".Replace("NUM}", int_expected_num.ToString()));
        }/*</VoiceArgcError(string str_method, int int_num_args, int int_expected_num)>*/

        public static string Eval(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            /*
                |eval
                    `JettisonAllCargoTriggerLogic
                    `if(BFS.ScriptSettings.ReadValue("bln_confirm") == "true"){BFS.Input.SendKeys("{DELETE}");BFS.Speech.TextToSpeech("Commander - All Cargo has been jettisoned, as ordered.");}else{BFS.Speech.TextToSpeech("Commander - The jettisonning of cargo has been belayed, as ordered.");
                    `<<NOTE:Here is where references go, if not using any, set to string.Empty:NOTE>>
            */
            var int_expected_num = 3;
            var returnValue = "ok";
            //>>>>>arr_arguments[0]; //<<--Descriptive name of code block
            //>>>>>arr_arguments[1]; //<<--code block
            //>>>>>arr_arguments[2]; //<<--references block <<NOTE:If not using any, set to string.Empty:NOTE>>
            if (int_num_subparams == int_expected_num)//>>>>>execute
            {
                var str_code_block_name = arr_arguments[0];
                var str_code_block = arr_arguments[1];
                var str_code_block_refs = arr_arguments[2].ToLower() != "null" ? arr_arguments[2] : "";
                Includes.VoiceBotSupportClasses.ScriptEngine.Eval(windowHandle, str_code_block_name, str_code_block, str_code_block_refs);
            }
            else //>>>>>warn
            {
                VoiceArgcError(windowHandle, "EVAL", int_num_subparams, int_expected_num);
            }//</if ( int_num_subparams == int_expected_num )>
            return returnValue;
        }

        /*
        -Verbal Confirmation Prompt(VCP): used typically for simple confirmation prompting
        arr_arguments[0] //<<--storage variable for the VCP response, will be a boolean
        arr_arguments[1] //<<--Aural Confirmation Prompt verbiage
        arr_arguments[2] //<<--ValidTrue ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        arr_arguments[3] //<<--ValidFalse ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        arr_arguments[4] //<<--Retries ~ delimited string defining all retry responses to the user prompting them to try again using a response from the valid list of responses. use and empty response to skip retry response attempts, ie give a retry response every 2 retries... .
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
        /// <param name="arr_arguments"></param>
        ///>>>>>arr_arguments[0] //<<--storage variable for the VCP response, will be a boolean
        ///>>>>>arr_arguments[1] //<<--Aural Confirmation Prompt verbiage
        ///>>>>>arr_arguments[2] //<<--ValidTrue ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        ///>>>>>arr_arguments[3] //<<--ValidFalse ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        ///>>>>>arr_arguments[4] //<<--Retries ~ delimited string defining all retry responses to the user prompting them to try again using a response from the valid list of responses. use and empty response to skip retry response attempts, ie give a retry response every 2 retries... .
        /// <param name="int_num_subparams"></param>
        /// <param name="int_expected_num"></param>
        public static string VerbalConfirmationPrompt(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            var returnValue = "ok";
            var int_expected_num = 5;
            //>>>>>validate
            if (int_num_subparams == int_expected_num)//>>>>>execute
            {
                var str_method = arr_arguments[1];
                var arr_str_valid_true = arr_arguments[2].Split('~');
                var arr_str_valid_false = arr_arguments[3].Split('~');
                var arr_str_retry_prompts = arr_arguments[4].Split('~');
                var str_user_response = "";
                var str_retry_prompt = arr_arguments[1];
                var bln_quit = false;
                var bln_valid_response = false;
                var bln_confirmation = true;
                var int_retries = 0;
                var int_retries_left = 0;
                //
                BFS.Speech.TextToSpeech(arr_arguments[1]);

                while (!bln_valid_response && !bln_quit)
                {
                    str_user_response = (string)Includes.Speech.SpeechRecognizer(str_retry_prompt);//<<--we've moved the voice prompt out side the loop so the system waits for a valid response without further "prompt"
                    BFS.Speech.TextToSpeech("I heard: " + str_user_response);
                    //
                    foreach (string str_valid_true in arr_str_valid_true)
                    {
                        //
                        if (str_user_response.Trim().ToLower() == str_valid_true.Trim().ToLower())
                        {
                            bln_valid_response = true;
                            bln_confirmation = true;
                            break;//<<--breach the loop
                        }
                        else {; }
                        //</if(str_user_response.ToLower() == str_valid_site.ToLower())>
                    }//</foreach(string str_valid_site in str_valid_sites.Split(','))>
                    if (!bln_valid_response)
                    {
                        //
                        foreach (string str_valid_false in arr_str_valid_false)
                        {
                            //
                            if (str_user_response.Trim().ToLower() == str_valid_false.Trim().ToLower())
                            {
                                bln_valid_response = true;
                                bln_confirmation = false;
                                break;//<<--breach the loop
                            }
                            else {; }
                            //</if(str_user_response.ToLower() == str_valid_site.ToLower())>
                        }//</foreach(string str_valid_site in str_valid_sites.Split(','))>
                    }
                    else {; }
                    //>>>>>was the response valid?
                    if (!bln_valid_response)
                    {
                        int_retries++;//<<--increment failsafe value, this will allow the prompt to fail after so many retries
                        int_retries_left = (arr_str_retry_prompts.Length - int_retries);
                        bln_quit = (int_retries_left <= -1);
                        //>>>>>see if we want to provide "help" to the user...
                        str_retry_prompt = (!bln_quit ? arr_str_retry_prompts[int_retries - 1] : "");
                    }
                    else
                    {
                        break;//<<--breach the loop
                    }
                    //</if(!bln_valid_response)>
                }//</while(!bln_valid_response || !bln_quit)>

                //</Complex - validated response>
                if (bln_valid_response)
                {
                    BFS.Speech.TextToSpeech("Commander, response accepted.");
                    BFS.ScriptSettings.WriteValue(arr_arguments[0], (bln_confirmation ? "true" : "false"));
                    returnValue += ":" + bln_confirmation.ToString();
                }
                else { returnValue += ":" + bln_confirmation.ToString(); }//>>>>>rejected
                                                                          //</if(bln_valid_response)>
            }
            else //>>>>>warn
            {
                VoiceArgcError(windowHandle, "", int_num_subparams, int_expected_num);
                returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
            }//</if ( int_num_subparams == int_expected_num )>
            return returnValue;
        }//</VerbalConfirmationPrompt>

        /*
        arr_arguments[0] //<<--storage variable for the VCP response, will be a boolean
        arr_arguments[1] //<<--Aural Input Prompt verbiage
        arr_arguments[2] //<<--ValidAccept ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        arr_arguments[3] //<<--ValidCancel ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        Example use and parameter organization:
        Calling macro is called NameShip, a non-toggle macro requiring verbal input of the ships name.
        An IPC message is setup to have the AOSCore process the VIP request and responding action
        "NameShip
            |VIP
                `str_ship_designation <<NOTE:the storage variable for the VCP response will be a boolean:NOTE>>
                `Commander - Please state ships new designation. Please finish with Accept to accept your verbal input and process.
                }
            |Eval
                `BFS.Speech.TextToSpeech("Commander - Ship name set to All Cargo has been jettisoned, as ordered.");}else{BFS.Speech.TextToSpeech("Commander - The jettisoning of cargo has been belayed, as ordered.");}
        "
        */

        /// <summary>
        /// Verbal Input Prompt(VIP): used typically for aquiring verbal dictation type responses
        /// </summary>
        /// <param name="arr_arguments"></param>
        ///>>>>>arr_arguments[0] //<<--storage variable for the VIP response, will be a boolean
        ///>>>>>arr_arguments[1] //<<--Aural Input Prompt verbiage
        ///>>>>>arr_arguments[2] //<<--ValidAccept ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        ///>>>>>arr_arguments[3] //<<--ValidCancel ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        /// <param name="int_num_subparams"></param>
        /// <param name="int_expected_num"></param>
        public static string VerbalInputPrompt(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            var returnValue = "ok";
            var int_expected_num = 4;
            //>>>>>validate
            if (int_num_subparams == int_expected_num)//>>>>>execute
            {
                var arr_str_valid_accept = "Accept~Submit~Confirm~Done".Split('~');
                var arr_str_valid_cancel = "Abort~Cancel".Split('~');
                var str_user_response = "";
                var str_result = "";
                var bln_valid_response = true;
                var bln_quit = false;
                var bln_accepted = false;

                BFS.Speech.TextToSpeech(arr_arguments[0]);//<<--initial aural prompt

                while (!bln_quit)
                {
                    str_user_response = (string)Includes.Speech.SpeechRecognizer("Ready");
                    BFS.Speech.TextToSpeech("I heard: " + str_user_response);
                    //>>>>>parse spoken data against list of valid accept tokens to see if content is complete (accepted)
                    foreach (string str_valid_accept in arr_str_valid_accept)
                    {
                        //
                        if (str_user_response.Trim().ToLower() == str_valid_accept.Trim().ToLower())
                        {
                            bln_accepted = true;
                            break;//<<--breach the loop
                        }
                        else {; }
                        //</if(str_user_response.ToLower() == str_valid_site.ToLower())>
                        //>>>>>parse spoken data against list of valid cancel tokens to see if content is complete (cancelled)
                        foreach (string str_valid_cancel in arr_str_valid_cancel)
                        {
                            //
                            if (str_user_response.Trim().ToLower() == str_valid_cancel.Trim().ToLower())
                            {
                                bln_valid_response = false;
                                break;//<<--breach the loop
                            }
                            else {; }
                            //</if(str_user_response.ToLower() == str_valid_site.ToLower())>
                        }//</foreach(string str_valid_site in str_valid_sites.Split(','))>
                         //>>>>>if response is not valid, the input was accepted as a cancel token
                        if (!bln_valid_response)
                        {
                            bln_quit = true;
                        }
                        else//>>>>>response is valid, response was either a command token to accept, or more input, look deeper...
                        {
                            if (!bln_accepted)//>>>>>if accepted is true, the input was the accept token, breech the loop
                            { bln_quit = true; }
                            else//>>>>>append user input to the resulting speech to text buffer
                            { str_result += (str_result.Length > 0 ? " " : "") + str_user_response; }
                            //</if(!bln_accepted)>
                        }
                        //</if(!bln_valid_response)>
                    }//</while(!bln_quit)>
                     //>>>>>if accepted, store value in VoiceBot variable supplied is arg0
                    if (bln_accepted)
                    {
                        BFS.Speech.TextToSpeech("Commander, response accepted.");
                        BFS.ScriptSettings.WriteValue(arr_arguments[0], str_result);
                        returnValue += ":" + str_result;
                    }
                    else { returnValue = "cancelled"; }//>>>>>do nothing
                                                       //</if(bln_accepted)>
                }//</foreach ( string str_valid_accept in arr_str_valid_accept )>
            }//</true>
            else //>>>>>warn
            {
                VoiceArgcError(windowHandle, "Verbal Input Prompt(VIP)", int_num_subparams, int_expected_num);
                returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
            }//</if ( int_num_subparams == int_expected_num )>
            return returnValue;
        }//</VerbalInputPrompt>

        /*
        arr_arguments[0] //<<--storage variable for the VCP response, will be a boolean
        arr_arguments[1] //<<--Aural Input Prompt verbiage
        arr_arguments[2] //<<--ValidAccept ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        arr_arguments[3] //<<--ValidCancel ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        Example use and parameter organization:
        Calling macro is called NameShip, a non-toggle macro requiring verbal input of the ships name.
        An IPC message is setup to have the AOSCore process the VIP request and responding action
        "NameShip
            |VSP
                `str_system_name <<NOTE:the storage variable for the VCP response will be a boolean:NOTE>>
                `Commander - Please spell out the name of the system. Please finish with Accept to accept your verbal input and process.
                }
            |Eval
                `BFS.Speech.TextToSpeech("Commander - Ship name set to All Cargo has been jettisoned, as ordered.");}else{BFS.Speech.TextToSpeech("Commander - The jettisoning of cargo has been belayed, as ordered.");}
        "
        */

        /// <summary>
        /// Verbal Spell Prompt(VSP): used typically for prompts that may type the response into an input field, and requires greater accuracy on the spelling of the input (typically a single word)
        /// </summary>
        /// <param name="arr_arguments"></param>
        ///>>>>>arr_arguments[0] //<<--storage variable for the VSP response, will be a boolean
        ///>>>>>arr_arguments[1] //<<--Aural Input Prompt verbiage
        ///>>>>>arr_arguments[2] //<<--ValidAccept ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        ///>>>>>arr_arguments[3] //<<--ValidCancel ~ delimited string defining all valid true responses (Note speach to text translation is being done, so spelling may be important, and may need to be phonetic, debug by sampling what the recognizer hears and use that.)
        /// <param name="int_num_subparams"></param>
        /// <param name="int_expected_num"></param>
        public static string VerbalSpellPrompt(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            var returnValue = "ok";
            var int_expected_num = 4;
            //>>>>>validate
            if (int_num_subparams == int_expected_num)//>>>>>execute
            {
                var arr_str_valid_accept = "Accept~Submit~Confirm~Done".Split('~');
                var arr_str_valid_cancel = "Abort~Cancel".Split('~');
                var str_user_response = "";
                var str_result = "";
                var bln_valid_response = true;
                var bln_quit = false;
                var bln_accepted = false;

                BFS.Speech.TextToSpeech(arr_arguments[0]);//<<--initial aural prompt

                while (!bln_quit)
                {
                    str_user_response = (string)Includes.Speech.SpeechRecognizer("Ready");
                    BFS.Speech.TextToSpeech("I heard: " + str_user_response);
                    //>>>>>parse spoken data against list of valid accept tokens to see if content is complete (accepted)
                    foreach (string str_valid_accept in arr_str_valid_accept)
                    {
                        //
                        if (str_user_response.Trim().ToLower() == str_valid_accept.Trim().ToLower())
                        {
                            bln_accepted = true;
                            break;//<<--breach the loop
                        }
                        else {; }
                        //</if(str_user_response.ToLower() == str_valid_site.ToLower())>
                        //>>>>>parse spoken data against list of valid cancel tokens to see if content is complete (cancelled)
                        foreach (string str_valid_cancel in arr_str_valid_cancel)
                        {
                            //
                            if (str_user_response.Trim().ToLower() == str_valid_cancel.Trim().ToLower())
                            {
                                bln_valid_response = false;
                                break;//<<--breach the loop
                            }
                            else {; }
                            //</if(str_user_response.ToLower() == str_valid_site.ToLower())>
                        }//</foreach(string str_valid_site in str_valid_sites.Split(','))>
                         //>>>>>if response is not valid, the input was accepted as a cancel token
                        if (!bln_valid_response)
                        {
                            bln_quit = true;
                        }
                        else//>>>>>response is valid, response was either a command token to accept, or more input, look deeper...
                        {
                            if (!bln_accepted)//>>>>>if accepted is true, the input was the accept token, breech the loop
                            { bln_quit = true; }
                            else//>>>>>append user input to the resulting speech to text buffer
                            { str_result += (str_result.Length > 0 ? " " : "") + str_user_response; }
                            //</if(!bln_accepted)>
                        }
                        //</if(!bln_valid_response)>
                    }//</while(!bln_quit)>
                     //>>>>>if accepted, store value in VoiceBot variable supplied is arg0
                    if (bln_accepted)
                    {
                        BFS.Speech.TextToSpeech("Commander, response accepted.");
                        BFS.ScriptSettings.WriteValue(arr_arguments[0], str_result);
                        returnValue += ":" + str_result;
                    }
                    else { returnValue = "cancelled"; }//>>>>>do nothing
                                                       //</if(bln_accepted)>
                }//</foreach ( string str_valid_accept in arr_str_valid_accept )>
            }//</true>
            else //>>>>>warn
            {
                VoiceArgcError(windowHandle, "Verbal Input Prompt(VIP)", int_num_subparams, int_expected_num);
                returnValue = "error:{ERROR}".Replace("{ERROR}", "Invalid number of arguments.");
            }//</if ( int_num_subparams == int_expected_num )>
            return returnValue;
        }//</VerbalSpellPrompt>

        public static string TextToSpeech(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            var str_method = arr_arguments[1];
            var int_expected_num = 1;
            var returnValue = "ok";
            //>>>>>arr_arguments[0]; //<<--message to speak
            if (int_num_subparams == int_expected_num)//>>>>>execute
            {
                BFS.Speech.TextToSpeech(arr_arguments[0]);
            }
            else //>>>>>warn
            {
                VoiceArgcError(windowHandle, str_method.ToUpper(), int_num_subparams, int_expected_num);
            }//</if ( int_num_subparams == int_expected_num )>
            return returnValue;
        }

        public static string InitShipState(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            var returnValue = "ok";
            Includes.ShipModel.ResetShipState();
            return returnValue;
        }

        public static string GetShipState(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            var str_method = arr_arguments[1];
            var int_expected_num = 1;
            var returnValue = "ok";
            //>>>>>arr_arguments[0]; //<<--var to get
            if (int_num_subparams == int_expected_num)//>>>>>execute
            {
                if (BFS.ScriptSettings.ReadValue("ShipState").Length < 1) { Includes.ShipModel.ResetShipState(); } else {; }//<<--init if not present
                returnValue = Includes.ShipModel.GetState(arr_arguments[0]);
            }
            else //>>>>>warn
            {
                VoiceArgcError(windowHandle, str_method.ToUpper(), int_num_subparams, int_expected_num);
            }//</if ( (arr_arguments.Length - 1) == 2)>
            return returnValue;
        }

        public static string SetShipState(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            var str_method = arr_arguments[1];
            var int_expected_num = 2;
            var returnValue = "ok";
            //>>>>>arr_arguments[0]; //<<--var to set
            //>>>>>arr_arguments[1]; //<<--value to set var to
            if (int_num_subparams == int_expected_num)//>>>>>execute
            {
                if (BFS.ScriptSettings.ReadValue("ShipState").Length < 1) { Includes.ShipModel.ResetShipState(); } else {; }//<<--init if not present
                Includes.ShipModel.SetState(arr_arguments[0], arr_arguments[1]);
            }
            else //>>>>>warn
            {
                VoiceArgcError(windowHandle, str_method.ToUpper(), int_num_subparams, int_expected_num);
            }//</if ( int_num_subparams == int_expected_num )>
            return returnValue;
        }

        public static string SetValue(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            var str_method = arr_arguments[1];
            var int_expected_num = 2;
            var returnValue = "ok";
            //>>>>>arr_arguments[0]; //<<--var to set
            //>>>>>arr_arguments[1]; //<<--value to set var to
            if (int_num_subparams == int_expected_num)//>>>>>execute
            {
                Includes.FxLib.SaveArgs(arr_arguments[0], arr_arguments[1].Split('|'));
            }
            else //>>>>>warn
            {
                VoiceArgcError(windowHandle, str_method.ToUpper(), int_num_subparams, int_expected_num);
            }//</if ( int_num_subparams == int_expected_num )>
            return returnValue;
        }

        public static string SendKeys(IntPtr windowHandle, string[] arr_arguments, int int_num_subparams)
        {
            var str_method = arr_arguments[1];
            var int_expected_num = 1;
            var returnValue = "ok";
            //>>>>>arr_arguments[0]; //<<--key-sequence to send
            if (int_num_subparams == int_expected_num)//>>>>>execute
            {
                BFS.Input.SendKeys(arr_arguments[0]);
            }
            else //>>>>>warn
            {
                VoiceArgcError(windowHandle, str_method.ToUpper(), int_num_subparams, int_expected_num);
            }//</if ( int_num_subparams == int_expected_num )>
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
                public static string str_default_references = "System.dll | System.Core.dll |System.Data.dll | System.Drawing.dll | System.Management.dll | System.Web.dll | System.Windows.Forms.dll | System.Xml.dll | mscorlib.dll |  C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\Microsoft.CSharp.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Speech.dll | C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\Profile\\Client\\System.Data.Linq.dll".Replace("\\", "\\\\");

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
                    str_code = str_code.Replace("{REFERENCES}", Includes.VoiceBotSupportClasses.Constants.str_default_references);
                    sb_script_core.Append(str_code);//</sb_script_core>
                    Assembly assembly_trigger_logic = (Assembly)ScriptEngine.CsCodeAssembler(windowHandle, str_code_block_name, sb_script_core.ToString(), str_references = "");
                    obj_parameters_array = new object[] { };
                    dynamic obj_class = CreateClassInstance(assembly_trigger_logic, str_class_name, obj_parameters_array);
                    object obj_return = obj_class.Run(windowHandle);
                    //object obj_return = CallFunction(assembly_trigger_logic, str_function_name, obj_parameters_array);
                    return (obj_return);
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
                public static Assembly CsCodeAssembler(IntPtr windowHandle, string str_code_block_name, string str_source_cs, string str_references = "")
                {
                    Assembly obj_return = null;
                    str_references = (str_references.Length != 0 ? str_references : Includes.VoiceBotSupportClasses.Constants.str_default_references);
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
                    str_references += (str_references.Length > 0 ? "|" : "") + "C:\\Program Files (x86)\\VoiceBot\\VoiceBot.exe";//<<--add BFS library to the references
                    foreach (string str_reference in str_references.Split('|'))
                    {
                        System.Diagnostics.Debug.WriteLine("Adding reference: " + str_reference.Trim());
                        cp_compiler_params.ReferencedAssemblies.Add(str_reference.Trim());
                    }//</foreach(string str_reference in str_references.Split('|'))>
                     //
                    StringBuilder sb_script_core = new StringBuilder("");//<<--more flexible than string concatination if we add hardcoded library logic here
                                                                         //>>>>>ScriptCore.Includes.INCLUDE_CLASS.INCLUDE_FUNCTION
                    sb_script_core.Append(str_source_cs);//</sb_script_core>
                                                         //>>>>>attempt to compile
                    CompilerResults cr_result = provider.CompileAssemblyFromSource(cp_compiler_params, sb_script_core.ToString());
                    //
                    if (cr_result.Errors.Count > 0)//>>>>>report error, and fail
                    {
                        foreach (CompilerError CompErr in cr_result.Errors)
                        {
                            string str_line = sb_script_core.ToString().Split('\n')[CompErr.Line - (CompErr.Line > 0 ? 1 : 0)];
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
                    return (obj_return);
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
                    var obj_include = ScriptEngine.CsCodeAssembler(windowHandle, str_include, str_code, str_references);
                    return ((object)obj_include);
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
                    return (str_return);
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
                    return (str_return);
                }//</Base64Decode(string str_content)>

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
                        if (str_code.Length > 0)
                        { obj_include = ScriptEngine.CsCodeAssembler(windowHandle, str_include_path.Split('\\')[str_include_path.Split('\\').Length - 1].Replace(".cs", ""), str_code, str_references); }
                        else
                        {; }
                        //</if(str_code.Length > 0)>
                    }
                    catch (Exception error)
                    {
                        System.Diagnostics.Debug.WriteLine(error.Message);
                    }
                    //</try>
                    return ((object)obj_include);
                }//</GetInclude(IntPtr windowHandle, string str_include,string str_references = "")>

                //-------------------------------------------------------------------
            }/*</class::ScriptEngine>*/

            //=====================================================================
            //== /ScriptEngine Function Library
            //=====================================================================

            #endregion ScriptEngine

            //=====================================================================
        }/*</namespace::VoiceBotSupportClasses>*/

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
            private static Type type_ShipModel = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_EliteDangerousShipModel", Includes.VoiceBotSupportClasses.Constants.str_default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.ShipModel")).ToArray<Type>()[0];

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
            public static Assembly assem_FxLib = (Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_DarkLibs", Includes.VoiceBotSupportClasses.Constants.str_default_references);
            public static Type type_FxLib = assem_FxLib.GetTypes().Where(x => x.FullName.Contains("Includes.FxLib")).ToArray<Type>()[0];
            public delegate Dictionary<int, KeyValuePair<string, string>> GetArgsDelegate(string str_variable_name);

            public delegate void SaveArgsDelegate(string str_variable_name, string[] arr_content);

            //>>>>>staff them
            public static GetArgsDelegate GetArgs = (GetArgsDelegate)Delegate.CreateDelegate(typeof(GetArgsDelegate), (((type_FxLib.GetMethods().Where(x => x.Name == "GetArgs")).ToArray<MethodInfo>()))[0]);

            public static SaveArgsDelegate SaveArgs = (SaveArgsDelegate)Delegate.CreateDelegate(typeof(SaveArgsDelegate), (((type_FxLib.GetMethods().Where(x => x.Name == "SaveArgs")).ToArray<MethodInfo>()))[0]);
            //>>>>>appoint delagates (signatures)
            //public delegate Includes.VoiceBotSupportClasses.Args GetArgsDelegate(string str_variable_name);
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
            private static Type type_Speech = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_DarkLibs", Includes.VoiceBotSupportClasses.Constants.str_default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.Speech")).ToArray<Type>()[0];

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
            private static Type type_DBS = ((Assembly)Includes.VoiceBotSupportClasses.ScriptEngine.LoadInclude(new IntPtr(), "Include_DarkLibs", Includes.VoiceBotSupportClasses.Constants.str_default_references)).GetTypes().Where(x => x.FullName.Contains("Includes.DBS")).ToArray<Type>()[0];

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

    #endregion Includes

    //=============================================================================
}/*</namespace::VoiceBotScriptTemplate>*/