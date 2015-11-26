using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MainClass
{
    public class ImageGUIDReference
    {
        public String DefaultImage { get; set; }
        public String BasePath { get; set; }
        public String GUID { get; set; }
        public ImageGUIDReference(String basePath, String defautImage)
        {
            this.BasePath = basePath;
            this.DefaultImage = defautImage;
            GUID = "";
        }

        public String GetURL()
        {
            String url;
            if (String.IsNullOrEmpty(GUID))
                url = BasePath + DefaultImage;
            else
                url = BasePath + GUID + ".png";
            return url;
        }

        public String GetURI()
        {
            String uri;
            if (String.IsNullOrEmpty(GUID))
                uri = "";
            else
                uri = BasePath + GUID + ".png";
            return uri;
        }

        public String GetImageURL(String GUID)
        {
            String url;
            if (String.IsNullOrEmpty(GUID))
                url = BasePath + DefaultImage;
            else
                url = BasePath + GUID + ".png";
            return url;
        }

        public String UpLoadImage(HttpRequestBase Request, String PreviousGUID)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    GUID = PreviousGUID;
                    if (!String.IsNullOrEmpty(GUID))
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(GetURI()));
                    }
                    GUID = Guid.NewGuid().ToString();
                    file.SaveAs(HttpContext.Current.Server.MapPath(GetURI()));
                    return GUID;
                }
            }
            return PreviousGUID;
        }

        public void Remove(String GUID)
        {
            if (!String.IsNullOrEmpty(GUID))
            {
                this.GUID = GUID;
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(GetURI()));
            }
        }
    }

    public class Programme
    {
        //ID de base de la BD
        public long Id { get; set; }

        //Nom du programme
        [Display(Name = "Nom du programme")]
        public String NomProgramme { get; set; }

        //Code du programme
        [Display(Name = "Code du programme")]
        public String CodeProgramme { get; set; }

        //Photo du programme
        [Display(Name = "Photo du programme")]
        public String Photo { get; set; }
        private ImageGUIDReference ImageReference;

        public Programme() 
        {
            NomProgramme = "";
            CodeProgramme = "";
            Photo = "";
            ImageReference = new ImageGUIDReference(@"/Images/Programmes/", @"anonymous.jpg");
        }

        public String GetPosterURL()
        {
            return ImageReference.GetImageURL(Photo);
        }

        public void UpLoadPoster(HttpRequestBase Request)
        {
            Photo = ImageReference.UpLoadImage(Request, Photo);
        }

        public void RemovePoster()
        {
            ImageReference.Remove(Photo);
        }
    }

    public class Programmes : SqlExpressUtilities.SqlExpressWrapper
    {
        public Programme programme { get; set; }

        public Programmes(object cs) : base(cs)
        {
            programme = new Programme();
        }

        public Programmes() { programme = new Programme(); }

        public List<Programme> ToList()
        {
            List<object> list = this.RecordsList();
            List<MainClass.Programme> programs_list = new List<Programme>();
            foreach (Programme programme in list)
            {
                programs_list.Add(programme);
            }
            return programs_list;
        }

        public override void DeleteRecordByID(String ID)
        {
            if (this.SelectByID(ID))
            {
                this.programme.RemovePoster();
                base.DeleteRecordByID(ID);
            }
        }


    }
}