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
        public const string SCHOOL_INFOTEMPLATE_CONTENT = "<b>Nome:</b> ${Name}<br/><b>Address:</b> ${Address}<br/>";
        public const string COPS_INFOTEMPLATE_TITLE = "Posto Policial";
        public const string COPS_INFOTEMPLATE_CONTENT = "<b>Nome:</b> ${Name}<br/><b>Address:</b> ${Address}<br/>";
        #endregion
    }
}
