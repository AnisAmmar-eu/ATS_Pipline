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
    public static class DLLvision
    {
        const string dllPath = "fcx_ats_lib.dll";

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);

        //Initalisation de la librairie avec fichier de config
        [DllImport(
            dllPath,
            EntryPoint = "ats_init",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_init(string configFilename);

        //Désinitialisation de la librairie
        [DllImport(dllPath, EntryPoint = "ats_deinit", CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_deinit();

        //Enregistrement d'un fichier de paramètrage dynamique pour le matching
        [DllImport(
            dllPath,
            EntryPoint = "ats_register_match_params_dynamic",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_register_match_params_dynamic(long id, string match_parm_dyn_filename);

        //Enregistrement d'un fichier de paramètrage statique pour signature
        [DllImport(
            dllPath,
            EntryPoint = "ats_register_sign_params_static",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_register_sign_params_static(long id, string sign_parm_static_filename);

        //Enregistrement d'un fichier de paramètrage dynamique pour signature
        [DllImport(
            dllPath,
            EntryPoint = "ats_register_sign_params_dynamic",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_register_sign_params_dynamic(long id, string sign_parm_dynamic_filename);

        //Désenregistrement d'un fichier de paramètrage dynamique pour matching
        [DllImport(
            dllPath,
            EntryPoint = "ats_unregister_match_params_dynamic",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_unregister_match_params_dynamic(long id);

        //Désenregistrement d'un fichier de paramètrage statique pour signature
        [DllImport(
            dllPath,
            EntryPoint = "ats_unregister_sign_params_static",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_unregister_sign_params_static(long id);

        //Désenregistrement d'un fichier de paramètrage dynamique pour signature
        [DllImport(
            dllPath,
            EntryPoint = "ats_unregister_sign_params_dynamic",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_unregister_sign_params_dynamic(long id);

        //Enregistrement d'un dataset
        [DllImport(
            dllPath,
            EntryPoint = "ats_register_dataset",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_register_dataset(long dataset_id, long sign_param_static_id, int gpuId);

        //Enregistrement d'un dataset
        [DllImport(
            dllPath,
            EntryPoint = "ats_unregister_dataset",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_unregister_dataset(long dataset_id);

        //Signature
        [DllImport(
            dllPath,
            EntryPoint = "ats_sign",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_sign(
            long signParamsStaticId,
            long signParamsDynId,
            string in_directory,
            string anodeId,
            string out_director);

        //Chargement d'une anode
        [DllImport(
            dllPath,
            EntryPoint = "ats_load_anode",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_load_anode(long dataset_id, string in_directory, string anode_id);

        //Déchargement d'une anode
        [DllImport(
            dllPath,
            EntryPoint = "ats_drop_anode",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_drop_anode(long dataset_id, string anode_id);

        //Déchargement d'une anode
        [DllImport(dllPath, EntryPoint = "ats_drop_anode_all", CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_drop_anode_all(long dataset_id);

        //Matching
        [DllImport(
            dllPath,
            EntryPoint = "ats_match",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        static extern IntPtr fcx_match(long dataset_id, long matchParamsDynId, string in_directory, string anodeId);

        //Obtenir code erreur matching
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_errorCode",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_errorCode(IntPtr matchRet);

        //Obtenir id de l'anode
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_anodeId",
            CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr fcx_matchRet_anodeId(IntPtr matchRet);

        //Obtenir score de similarité de l'anode
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_similarityScore",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_similarityScore(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(dllPath, EntryPoint = "ats_matchRet_free", CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_free(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_set_log_type",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_set_log_type(string logType);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_activate_score_buffering",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_activate_score_buffering(string path_to_folder);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_activate_pipeline_debug",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode)]
        public static extern int fcx_activate_pipeline_debug(string path_to_folder);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_worstScore",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_worstScore(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_bestScore",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_bestScore(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_nbBests",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_nbBests(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_bestsIdx_anodeId",
            CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr fcx_matchRet_bestsIdx_anodeId(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_bestsIdx_score",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_bestsIdx_score(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(dllPath, EntryPoint = "ats_matchRet_mean", CallingConvention = CallingConvention.StdCall)]
        static extern double fcx_matchRet_mean(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_variance",
            CallingConvention = CallingConvention.StdCall)]
        static extern double fcx_matchRet_variance(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_elapsed",
            CallingConvention = CallingConvention.StdCall)]
        static extern long fcx_matchRet_elapsed(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_threshold",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_threshold(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            dllPath,
            EntryPoint = "ats_matchRet_cardinality_after_brut_force",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_cardinality_after_brut_force(IntPtr matchRet);
    }
}