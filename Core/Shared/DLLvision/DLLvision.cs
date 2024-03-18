using System;
using System.Runtime.InteropServices; // invoke c++ dll
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace Core.Shared.DLLvision
{
    public static partial class DLLvision
    {
        const string dllPath = "fcx_ats_lib.dll";

        [LibraryImport("kernel32.dll", EntryPoint = "SetDllDirectoryW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetDllDirectory(string lpPathName);

        //Initalisation de la librairie avec fichier de config
        [LibraryImport(
            dllPath, EntryPoint = "ats_initW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_init(string configFilename);

        //Désinitialisation de la librairie
        [LibraryImport(dllPath, EntryPoint = "ats_deinitW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_deinit();

        //Enregistrement d'un fichier de paramètrage dynamique pour le matching
        [LibraryImport(
            dllPath, EntryPoint = "ats_register_match_params_dynamicW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_register_match_params_dynamic(long id, string match_parm_dyn_filename);

        //Enregistrement d'un fichier de paramètrage statique pour signature
        [LibraryImport(
            dllPath, EntryPoint = "ats_register_sign_params_staticW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_register_sign_params_static(long id, string sign_parm_static_filename);

        //Enregistrement d'un fichier de paramètrage dynamique pour signature
        [LibraryImport(
            dllPath, EntryPoint = "ats_register_sign_params_dynamicW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_register_sign_params_dynamic(long id, string sign_parm_dynamic_filename);

        //Désenregistrement d'un fichier de paramètrage dynamique pour matching
        [LibraryImport(
            dllPath, EntryPoint = "ats_unregister_match_params_dynamicW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_unregister_match_params_dynamic(long id);

        //Désenregistrement d'un fichier de paramètrage statique pour signature
        [LibraryImport(
            dllPath, EntryPoint = "ats_unregister_sign_params_staticW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_unregister_sign_params_static(long id);

        //Désenregistrement d'un fichier de paramètrage dynamique pour signature
        [LibraryImport(
            dllPath, EntryPoint = "ats_unregister_sign_params_dynamicW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_unregister_sign_params_dynamic(long id);

        //Enregistrement d'un dataset
        [LibraryImport(
            dllPath, EntryPoint = "ats_register_datasetW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_register_dataset(long dataset_id, long sign_param_static_id, int gpuId);

        //Enregistrement d'un dataset
        [LibraryImport(
            dllPath, EntryPoint = "ats_unregister_datasetW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_unregister_dataset(long dataset_id);

        //Signature
        [LibraryImport(
            dllPath, EntryPoint = "ats_signW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_sign(
            long signParamsStaticId,
            long signParamsDynId,
            string in_directory,
            string anodeId,
            string out_director);

        //Chargement d'une anode
        [LibraryImport(
            dllPath, EntryPoint = "ats_load_anodeW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_load_anode(long dataset_id, string in_directory, string anode_id);

        //Déchargement d'une anode
        [LibraryImport(
            dllPath, EntryPoint = "ats_drop_anodeW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_drop_anode(long dataset_id, string anode_id);

        //Déchargement d'une anode
        [LibraryImport(dllPath, EntryPoint = "ats_drop_anode_allW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_drop_anode_all(long dataset_id);

        //Matching
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        static partial IntPtr fcx_match(long dataset_id, long matchParamsDynId, string in_directory, string anodeId);

        //Obtenir code erreur matching
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_errorCodeW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_matchRet_errorCode(IntPtr matchRet);

        //Obtenir id de l'anode
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_anodeIdW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        static partial IntPtr fcx_matchRet_anodeId(IntPtr matchRet);

        //Obtenir score de similarité de l'anode
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_similarityScoreW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_matchRet_similarityScore(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(dllPath, EntryPoint = "ats_matchRet_freeW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_matchRet_free(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_set_log_typeW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_set_log_type(string logType);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_activate_score_bufferingW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_activate_score_buffering(string path_to_folder);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_activate_pipeline_debugW", StringMarshalling = StringMarshalling.Utf16)]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_activate_pipeline_debug(string path_to_folder);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_worstScoreW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_matchRet_worstScore(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_bestScoreW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_matchRet_bestScore(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_nbBestsW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_matchRet_nbBests(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_bestsIdx_anodeIdW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        static partial IntPtr fcx_matchRet_bestsIdx_anodeId(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_bestsIdx_scoreW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_matchRet_bestsIdx_score(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(dllPath, EntryPoint = "ats_matchRet_meanW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        static partial double fcx_matchRet_mean(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_varianceW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        static partial double fcx_matchRet_variance(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_elapsedW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        static partial long fcx_matchRet_elapsed(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_thresholdW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_matchRet_threshold(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [LibraryImport(
            dllPath, EntryPoint = "ats_matchRet_cardinality_after_brut_forceW")]
        [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })]
        public static partial int fcx_matchRet_cardinality_after_brut_force(IntPtr matchRet);
    }
}