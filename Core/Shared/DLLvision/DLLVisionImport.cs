using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DLLVision
{
    public static class DLLVisionImport
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);

        //Obtient le nombre de messages de warning
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_warnings_nb_messages",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_warnings_nb_messages();

        //Récupère le message de warning
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_warnings_get_message",
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr fcx_warnings_get_message(int idx);

        //Retire tout les warnings
        [DllImport("fcx_ats_lib.dll", EntryPoint = "ats_warnings_clear", CallingConvention = CallingConvention.StdCall)]
        public static extern void fcx_warnings_clear();

        //Initalisation de la librairie
        [DllImport("fcx_ats_lib.dll", EntryPoint = "ats_init", CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_init();

        //Désinitialisation de la librairie
        [DllImport("fcx_ats_lib.dll", EntryPoint = "ats_deinit", CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_deinit();

        //Enregistrement d'un fichier de paramètrage dynamique pour le matching
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_register_match_params_dynamic",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_register_match_params_dynamic(long id, string match_parm_dyn_filename);

        //Enregistrement d'un fichier de paramètrage statique pour signature
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_register_sign_params_static",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_register_sign_params_static(long id, string sign_parm_static_filename);

        //Enregistrement d'un fichier de paramètrage dynamique pour signature
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_register_sign_params_dynamic",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_register_sign_params_dynamic(long id, string sign_parm_dynamic_filename);

        //Désenregistrement d'un fichier de paramètrage dynamique pour matching
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_unregister_match_params_dynamic",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_unregister_match_params_dynamic(long id);

        //Désenregistrement d'un fichier de paramètrage statique pour signature
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_unregister_sign_params_static",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_unregister_sign_params_static(long id);

        //Désenregistrement d'un fichier de paramètrage dynamique pour signature
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_unregister_sign_params_dynamic",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_unregister_sign_params_dynamic(long id);

        //Enregistrement d'un dataset
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_register_dataset",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_register_dataset(long dataset_id, long sign_param_static_id, int gpuId);

        //Enregistrement d'un dataset
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_unregister_dataset",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_unregister_dataset(long dataset_id);

        //Signature
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_sign",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Ansi)]
        public static extern int fcx_sign(
            long signParamsStaticId,
            long signParamsDynId,
            string in_directory,
            string anodeId,
            string out_director);

        //Chargement d'une anode
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_load_anode",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Ansi)]
        public static extern int fcx_load_anode(long dataset_id, string in_directory, string anode_id);

        //Déchargement d'une anode
        [DllImport("fcx_ats_lib.dll", EntryPoint = "ats_drop_anode", CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_drop_anode(long dataset_id, string anode_id);

        //Déchargement d'une anode
        [DllImport("fcx_ats_lib.dll", EntryPoint = "ats_drop_anode_all", CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_drop_anode_all(long dataset_id);

        //Matching
        [DllImport("fcx_ats_lib.dll", EntryPoint = "ats_match", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr fcx_match(
            long dataset_id,
            long matchParamsDynId,
            string in_directory,
            string anodeId);

        //Obtenir code erreur matching
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_errorCode",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_errorCode(IntPtr matchRet);

        //Obtenir id de l'anode
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_anodeId",
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr fcx_matchRet_anodeId(IntPtr matchRet);

        //Obtenir score de similarité de l'anode
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_similarityScore",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_similarityScore(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport("fcx_ats_lib.dll", EntryPoint = "ats_matchRet_free", CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_free(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport("fcx_ats_lib.dll", EntryPoint = "ats_set_log_type", CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_set_log_type(string logType);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_activate_score_buffering",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_activate_score_buffering(string path_to_folder);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_activate_pipeline_debug",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_activate_pipeline_debug(string path_to_folder);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_worstScore",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_worstScore(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_bestScore",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_bestScore(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_nbBests",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_nbBests(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_bestsIdx_anodeId",
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr fcx_matchRet_bestsIdx_anodeId(IntPtr matchRet, int idx);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_bestsIdx_score",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_bestsIdx_score(IntPtr matchRet, int idx);

        //Libération de l'objet de résultat de match
        [DllImport("fcx_ats_lib.dll", EntryPoint = "ats_matchRet_mean", CallingConvention = CallingConvention.StdCall)]
        public static extern double fcx_matchRet_mean(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_variance",
            CallingConvention = CallingConvention.StdCall)]
        public static extern double fcx_matchRet_variance(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_elapsed",
            CallingConvention = CallingConvention.StdCall)]
        public static extern long fcx_matchRet_elapsed(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_threshold",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_threshold(IntPtr matchRet);

        //Libération de l'objet de résultat de match
        [DllImport(
            "fcx_ats_lib.dll",
            EntryPoint = "ats_matchRet_cardinality_after_brut_force",
            CallingConvention = CallingConvention.StdCall)]
        public static extern int fcx_matchRet_cardinality_after_brut_force(IntPtr matchRet);
    }
}