using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Constants
    {
        #region ESRI
        public const int SPATIAL_REFERENCES_OF_POINT = 4326;
        public const string GRAPHIC_POINT_TYPE_PMS = "esriPMS";
        public const string SCHOOL_POINT_ICON_URL = "../../Images/schools_mark.png";
        public const string COPS_POINT_ICON_URL = "../../Images/cops_mark.png";
        public const int ICONS_TO_LAYER_HEIGHT = 20;
        public const int ICONS_TO_LAYER_WIDTH = 20;
        public const string SCHOOL_INFOTEMPLATE_TITLE = "Instituição de Ensino";
        public const string SCHOOL_INFOTEMPLATE_CONTENT = "<b>Nome:</b> ${Name}<br/><b>Endereço:</b> ${Address}<br/>";
        public const string COPS_INFOTEMPLATE_TITLE = "Posto Policial";
        public const string COPS_INFOTEMPLATE_CONTENT = "<b>Nome:</b> ${Name}<br/><b>Endereço:</b> ${Address}<br/>";
        public const string CRIMINAL_INDEX_CONTENT = "<strong>Bairro:</strong> ${District}<br /> <strong>Indice de Criminalidade:</strong> ${CriminalIndex}";
        public const string OUTLINE_TYPE = "esriSLS";
        public const string OUTLINE_STYLE = "esriSLSSolid";
        public const string POLYGON_TYPE = "esriSFS";
        public const string POLYGON_STYLE = "esriSFSSolid";
        public const int POLYGON_WIDTH = 1;
        #endregion

        #region COLORS
        public static int[] CRIMINAL_INDICE_VERY_LOW = new int[] {51, 255, 153, 0};
        public static int[] CRIMINAL_INDICE_LOW = new int[] { 0, 128, 255, 0 };
        public static int[] CRIMINAL_INDICE_MEDIUM = new int[] { 0, 0, 204, 0 };
        public static int[] CRIMINAL_INDICE_HIGH = new int[] { 204, 102, 0, 0 };
        public static int[] CRIMINAL_INDICE_VERY_HIGH = new int[] { 255, 0, 0, 0 };
        public const int COLOR_OUTLINE = 64;
        public const int COLOR_LINE = 255;
        #endregion
    }
}
