using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using netgarson.Entities;
using netgarson.Models;


namespace netgarson.Helpers
{
    public class InputControl
    {

        public static int LogoutControl()
        {
            int errorCode = 0;
            try
            {
                errorCode = 100;
                if (HttpContext.Current.Response.Cookies["loginValues"] != null)
                {
                    HttpContext.Current.Response.Cookies["loginValues"].Expires = DateTime.Now.AddDays(-1);
                }
                if (HttpContext.Current.Session["user"] != null)
                {
                    HttpContext.Current.Session["user"] = null;
                }
            }
            catch (Exception)
            {
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            return errorCode;
        }

        #region Login

        public static int LoginUserControl(string mail, string password, bool rememberMe)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                User user = model.SELECTUser_WHEREMailANDPassword(mail, password);
                if (user != null)
                {
                    errorCode = 100; //başarılı
                    System.Web.HttpContext.Current.Session["user"] = user;
                    if (rememberMe == true)
                    {
                        HttpCookie Cookie = null;
                        if (HttpContext.Current.Response.Cookies["loginValues"] != null)
                        {
                            Cookie = HttpContext.Current.Response.Cookies["loginValues"];
                        }
                        else
                        {
                            Cookie = new HttpCookie("loginValues");
                        }
                        Cookie.Expires = DateTime.Now.AddDays(30);
                        Cookie["mail"] = mail;
                        Cookie["password"] = password;
                        HttpContext.Current.Response.Cookies.Add(Cookie);
                    }
                    else
                    {
                        if (HttpContext.Current.Response.Cookies["loginValues"] != null)
                        {
                            HttpContext.Current.Response.Cookies["loginValues"].Expires = DateTime.Now.AddDays(-1);
                        }
                    }

                }
                else
                {
                    errorCode = 200; //kullanıcı adı veya şifre uyumsuz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        public static int LoginUserControl(string mail, string password)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                User user = model.SELECTUser_WHEREMailANDPassword(mail, password);
                if (user != null)
                {
                    errorCode = 100; //başarılı
                    System.Web.HttpContext.Current.Session["user"] = user;
                }
                else
                {
                    errorCode = 200; //kullanıcı adı veya şifre uyumsuz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        public static int SendRepasswordMailControl(string mail)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                User user = model.SELECTUser_WHEREMail(mail);
                if (user != null)
                {
                    MailQueue mailQueue = new MailQueue();
                    mailQueue.Mail = mail;
                    mailQueue.MailState = 1;
                    mailQueue.MailStateDescription = "beklemede";
                    mailQueue.ContentType = 0;
                    mailQueue.ContentTypeDescription = "şifremi unuttum";
                    mailQueue.RecordDate = DateTime.Now;
                    mailQueue.SendDate = null;

                    model.INSERTMailQueue(mailQueue);

                    errorCode = 100; //başarılı
                }
                else
                {
                    errorCode = 200; //geçersiz eposta
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region Index

        public static Tuple<int, List<int>> GetDashboardTotalCountListControl()
        {
            int errorCode = 0;
            Model model = new Model();
            List<int> countList = new List<int>();
            try
            {
                int user_ID = (HttpContext.Current.Session["user"] as User).ID;

                int scanCount = model.COUNTScan(user_ID);
                countList.Add(scanCount);
                int callCount = model.COUNTCall(user_ID);
                countList.Add(callCount);
                int cafeCommentCount = model.COUNTCafeComment(user_ID);
                countList.Add(cafeCommentCount);
                int menuCommentCount = model.COUNTMenuComment(user_ID);
                countList.Add(menuCommentCount);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, countList); ;
        }

        public static Tuple<int, List<string>> GetDashboardChartControl()
        {
            int errorCode = 0;
            Model model = new Model();
            Set set = new Set();
            List<string> chartValueList = new List<string>();
            try
            {
                int user_ID = (HttpContext.Current.Session["user"] as User).ID;
                set = model.SELECTSet(user_ID);

                if (set != null)
                {
                    chartValueList.Add(set.ScanChartType);
                    chartValueList.Add(set.CallChartType);
                    chartValueList.Add(DateTime.Now.Month.ToString());
                    chartValueList.Add(DateTime.Now.Year.ToString());

                    //if (set.ScanChartType == "daily")
                    //{
                    //    chartValueList.Add("daily");
                    //    chartValueList.Add(Convert.ToInt32(DateTime.Now.Month.ToString()).ToString());
                    //    chartValueList.Add(Convert.ToInt32(DateTime.Now.Year.ToString()).ToString());
                    //}
                    //else if(set.ScanChartType== "monthly")
                    //{
                    //    chartValueList.Add("monthly");
                    //    chartValueList.Add(Convert.ToInt32(DateTime.Now.Year.ToString()).ToString());
                    //}
                    //else if (set.ScanChartType == "yearly")
                    //{
                    //    chartValueList.Add("yearly");
                    //    chartValueList.Add(Convert.ToInt32(DateTime.Now.Year.ToString()).ToString());
                    //}

                    //if (set.CallChartType == "daily")
                    //{
                    //    chartValueList.Add("daily");
                    //    chartValueList.Add(Convert.ToInt32(DateTime.Now.Month.ToString()).ToString());
                    //    chartValueList.Add(Convert.ToInt32(DateTime.Now.Year.ToString()).ToString());
                    //}
                    //else if (set.CallChartType == "monthly")
                    //{
                    //    chartValueList.Add("monthly");
                    //    chartValueList.Add(Convert.ToInt32(DateTime.Now.Year.ToString()).ToString());
                    //}
                    //else if (set.CallChartType == "yearly")
                    //{
                    //    chartValueList.Add("yearly");
                    //    chartValueList.Add(Convert.ToInt32(DateTime.Now.Year.ToString()).ToString());
                    //}


                    errorCode = 100;
                }
                else
                {
                    errorCode = 200;
                }

            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, chartValueList); ;
        }

        public static Tuple<int, List<Year>> GetDashboardChartYearSelectControl()
        {
            int errorCode = 0;
            Model model = new Model();
            List<Year> yearList = new List<Year>();
            try
            {
                int user_ID = (HttpContext.Current.Session["user"] as User).ID;
                yearList = model.SELECTYear_ORDERBYValue();

                if (yearList != null)
                {
                    if(DateTime.Now.Year!= yearList.Select(m => m.Value).Max())
                    {
                        Year year = new Year();
                        year.Value = DateTime.Now.Year;
                        model.INSERTYear(year);
                        yearList.Add(year);
                    }
                    errorCode = 100;
                }
                else
                {
                    errorCode = 200;
                }

            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, yearList); ;
        }

        public static Tuple<int, List<YearDecade>> GetDashboardChartYearDecadeSelectControl()
        {
            int errorCode = 0;
            Model model = new Model();
            List<YearDecade> yearDecadeList = new List<YearDecade>();
            try
            {
                int user_ID = (HttpContext.Current.Session["user"] as User).ID;
                yearDecadeList = model.SELECTYearDecade_ORDERBYBeginValue();

                if (yearDecadeList != null)
                {
                    if (DateTime.Now.Year > yearDecadeList.Select(m => m.EndValue).Max())
                    {
                        YearDecade yearDecade = new YearDecade();
                        yearDecade.BeginValue = DateTime.Now.Year;
                        yearDecade.EndValue = DateTime.Now.Year + 9;
                        model.INSERTYearDecade(yearDecade);
                        yearDecadeList.Add(yearDecade);
                    }
                    errorCode = 100;
                }
                else
                {
                    errorCode = 200;
                }

            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, yearDecadeList); ;
        }

        #endregion

        #region MenuList

        public static int DeleteMenuControl(int ID)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                Menu menu = model.SELECTMenu_WHEREID(ID);
                model.DELETEMenu_WHEREID(ID);
                model.DELETECategoryMenuRel_WHERECategory_ID(ID);
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menu.ImagePath)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menu.ImagePath));
                }
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region NewMenu

        public static int SaveMenuControl(Menu menu, List<CategoryMenuRel> categoryMenuRelList)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (menu.Name != null && menu.Name != "")
                {
                    if (menu.ImagePath != null && menu.ImagePath != "")
                    {

                        if (menu.Price < 10000 && menu.Price >= 0)
                        {
                            menu.Price = Math.Round(menu.Price, 2);

                            if (File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/TempMenu/" + menu.ImagePath)))//görsel upload edilmiş mi
                            {
                                menu.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                                Menu menuNameControl = model.SELECTMenu_WHERENameANDUser_ID(menu.Name, menu.User_ID);
                                model.Close();
                                if (menuNameControl == null)//aynı isimde eklenmiş kategori varmı
                                {
                                    string tempDi = menu.ImagePath;
                                    menu.ImagePath = menu.ImagePath.Remove(0, 36);
                                    menu.ImagePath = menu.User_ID.ToString() + "-" + menu.Name + "-" + menu.ImagePath;

                                    int menuID = model.INSERTMenu_RETURNID(menu);
                                    if (categoryMenuRelList != null)
                                    {
                                        foreach (var categoryMenuRel in categoryMenuRelList)
                                        {
                                            categoryMenuRel.Menu_ID = menuID;
                                            model.INSERTCategoryMenuRel(categoryMenuRel);
                                        }
                                    }
                                    DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadImages/Menu"));
                                    File.Move(HttpContext.Current.Server.MapPath("/UploadImages/TempMenu/" + tempDi), HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menu.ImagePath));

                                    errorCode = 100;//başarılı
                                }
                                else
                                {
                                    errorCode = 600;//aynı isimde menu kaydedilemez.
                                }
                            }
                            else
                            {
                                errorCode = 500;//görsel upload edilmeden kaydedilemez.
                            }
                        }
                        else
                        {
                            errorCode = 400;//ücret sınır dışında
                        }
                    }
                    else
                    {
                        errorCode = 300;//görsel boş bırakılamaz
                    }
                }
                else
                {
                    errorCode = 200;//Name alanı boş olamaz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region EditMenu

        public static int UpdateMenuControl(Menu menu, List<CategoryMenuRel> categoryMenuRelList)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (menu.Name != null && menu.Name != "")
                {
                    if (menu.ImagePath != null && menu.ImagePath != "")
                    {
                        if (menu.Price < 10000 && menu.Price >= 0)
                        {
                            menu.Price = Math.Round(menu.Price, 2);

                            if (File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/TempMenu/" + menu.ImagePath)))//görsel upload edilmiş mi
                            {
                                menu.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                                Menu menuOld = model.SELECTMenu_WHEREID(menu.ID);

                                if (menuOld != null)
                                {
                                    if (menuOld.Name != menu.Name)
                                    {
                                        menu.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                                        Menu menuNameControl = model.SELECTMenu_WHERENameANDUser_ID(menu.Name, menu.User_ID);
                                        if (menuNameControl == null)//aynı isimde eklenmiş kategori varmı
                                        {
                                            string tempDi = menu.ImagePath;
                                            if (menuOld.ImagePath != menu.ImagePath)
                                            {
                                                menu.ImagePath = menu.ImagePath.Remove(0, 36);
                                                menu.ImagePath = menu.User_ID.ToString() + "-" + menu.Name + "-" + menu.ImagePath;
                                            }

                                            model.UPDATEMenu_WHEREID(menu);

                                            model.DELETECategoryMenuRel_WHEREMenu_ID(menu.ID);

                                            if (categoryMenuRelList != null)
                                            {
                                                foreach (var categoryMenuRel in categoryMenuRelList)
                                                {
                                                    categoryMenuRel.Menu_ID = menu.ID;
                                                    model.INSERTCategoryMenuRel(categoryMenuRel);
                                                }
                                            }

                                            DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadImages/Menu"));

                                            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menu.ImagePath)))
                                            {
                                                System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menu.ImagePath));
                                            }

                                            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menuOld.ImagePath)))
                                            {
                                                System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menuOld.ImagePath));
                                            }

                                            File.Move(HttpContext.Current.Server.MapPath("/UploadImages/TempMenu/" + tempDi), HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menu.ImagePath));

                                            errorCode = 100;//başarılı
                                        }
                                        else
                                        {
                                            errorCode = 700;//aynı isimde kategori kaydedilemez.
                                        }
                                    }
                                    else
                                    {
                                        string tempDi = menu.ImagePath;

                                        if (menuOld.ImagePath != menu.ImagePath)
                                        {
                                            menu.ImagePath = menu.ImagePath.Remove(0, 36);
                                            menu.ImagePath = menu.User_ID.ToString() + "-" + menu.Name + "-" + menu.ImagePath;
                                        }
                                        menu.User_ID = (HttpContext.Current.Session["user"] as User).ID;

                                        model.UPDATEMenu_WHEREID(menu);
                                        model.DELETECategoryMenuRel_WHEREMenu_ID(menu.ID);
                                        if (categoryMenuRelList != null)
                                        {
                                            foreach (var categoryMenuRel in categoryMenuRelList)
                                            {
                                                categoryMenuRel.Menu_ID = menu.ID;
                                                model.INSERTCategoryMenuRel(categoryMenuRel);
                                            }
                                        }

                                        DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadImages/Menu"));
                                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menu.ImagePath)))
                                        {
                                            System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menu.ImagePath));
                                        }

                                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menuOld.ImagePath)))
                                        {
                                            System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menuOld.ImagePath));
                                        }
                                        File.Move(HttpContext.Current.Server.MapPath("/UploadImages/TempMenu/" + tempDi), HttpContext.Current.Server.MapPath("/UploadImages/Menu/" + menu.ImagePath));

                                        errorCode = 100;//başarılı
                                    }
                                }
                                else
                                {
                                    errorCode = 600;//beklenmedik hata
                                }
                            }
                            else
                            {
                                errorCode = 500;//görsel upload edilmeden kaydedilemez.
                            }
                        }
                        else
                        {
                            errorCode = 400;//fiyat sınır dışında
                        }
                    }
                    else
                    {
                        errorCode = 300;//görsel boş bırakılamaz
                    }
                }
                else
                {
                    errorCode = 200;//Name alanı boş olamaz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        public static int DeleteMenuImageBeforeUpdate(string imagePath)
        {
            int errorCode = 0;
            try
            {
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/TempMenu/" + imagePath)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/TempMenu/" + imagePath));
                }
                errorCode = 100;
            }
            catch (Exception)
            {
                errorCode = 99;
            }
            return errorCode;
        }

        #endregion

        #region CategoryList

        public static int DeleteCategoryControl(int category_ID)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {

                int ID = category_ID;
                Category category = model.SELECTCategory_WHEREID(ID);
                if (File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + category.ImagePath)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + category.ImagePath));
                }
                model.DELETECategory_WHEREID(ID);
                model.DELETECategoryMenuRel_WHERECategory_ID(category_ID);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return errorCode;
        }

        public static Tuple<int, List<Menu>> AjaxGetMenuFromCategoryControl(int category_ID)
        {
            int errorCode = 0;
            Model model = new Model();
            List<Menu> menuList = new List<Menu>();
            try
            {
                menuList = model.SELECTMenu_WHERECategory_ID(category_ID);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, menuList); ;
        }

        #endregion

        #region NewCategory

        public static int SaveCategoryControl(Category category)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (category.Name != null && category.Name != "")
                {
                    if (category.ImagePath != null && category.ImagePath != "")
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/TempCategory/" + category.ImagePath)))//görsel upload edilmiş mi
                        {
                            category.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                            Category categoryNameControl = model.SELECTCategory_WHERENameANDUser_ID(category.Name, category.User_ID);
                            if (categoryNameControl == null)//aynı isimde eklenmiş kategori varmı
                            {
                                string tempDi = category.ImagePath;
                                category.ImagePath = category.ImagePath.Remove(0, 36);
                                category.ImagePath = category.User_ID.ToString() + "-" + category.Name + "-" + category.ImagePath;

                                model.INSERTCategory(category);

                                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadImages/Category"));
                                File.Move(HttpContext.Current.Server.MapPath("/UploadImages/TempCategory/" + tempDi), HttpContext.Current.Server.MapPath("/UploadImages/Category/" + category.ImagePath));

                                errorCode = 100;//başarılı
                            }
                            else
                            {
                                errorCode = 500;//aynı isimde kategori kaydedilemez.
                            }
                        }
                        else
                        {
                            errorCode = 400;//görsel upload edilmeden kaydedilemez.
                        }
                    }
                    else
                    {
                        errorCode = 300;//görsel boş bırakılamaz
                    }
                }
                else
                {
                    errorCode = 200;//Name alanı boş olamaz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region EditCategory

        public static int UpdateCategoryControl(Category category)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (category.Name != null && category.Name != "")
                {
                    if (category.ImagePath != null && category.ImagePath != "")
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/TempCategory/" + category.ImagePath)))//görsel upload edilmiş mi
                        {
                            category.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                            Category categoryOld = model.SELECTCategory_WHEREID(category.ID);

                            if (categoryOld != null)
                            {
                                if (categoryOld.Name != category.Name)
                                {
                                    category.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                                    Category categoryNameControl = model.SELECTCategory_WHERENameANDUser_ID(category.Name, category.User_ID);
                                    if (categoryNameControl == null)//aynı isimde eklenmiş kategori varmı
                                    {
                                        string tempDi = category.ImagePath;
                                        if (categoryOld.ImagePath != category.ImagePath)
                                        {
                                            category.ImagePath = category.ImagePath.Remove(0, 36);
                                            category.ImagePath = category.User_ID.ToString() + "-" + category.Name + "-" + category.ImagePath;
                                        }
                                        model.UPDATECategory_WHEREID(category);

                                        DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadImages/Category"));

                                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + category.ImagePath)))
                                        {
                                            System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + category.ImagePath));
                                        }

                                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + categoryOld.ImagePath)))
                                        {
                                            System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + categoryOld.ImagePath));
                                        }

                                        File.Move(HttpContext.Current.Server.MapPath("/UploadImages/TempCategory/" + tempDi), HttpContext.Current.Server.MapPath("/UploadImages/Category/" + category.ImagePath));

                                        errorCode = 100;//başarılı
                                    }
                                    else
                                    {
                                        errorCode = 600;//aynı isimde kategori kaydedilemez.
                                    }
                                }
                                else
                                {
                                    string tempDi = category.ImagePath;

                                    if (categoryOld.ImagePath != category.ImagePath)
                                    {
                                        category.ImagePath = category.ImagePath.Remove(0, 36);
                                        category.ImagePath = category.User_ID.ToString() + "-" + category.Name + "-" + category.ImagePath;
                                    }
                                    category.User_ID = (HttpContext.Current.Session["user"] as User).ID;

                                    model.UPDATECategory_WHEREID(category);

                                    DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadImages/Category"));
                                    if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + category.ImagePath)))
                                    {
                                        System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + category.ImagePath));
                                    }

                                    if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + categoryOld.ImagePath)))
                                    {
                                        System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/Category/" + categoryOld.ImagePath));
                                    }
                                    File.Move(HttpContext.Current.Server.MapPath("/UploadImages/TempCategory/" + tempDi), HttpContext.Current.Server.MapPath("/UploadImages/Category/" + category.ImagePath));

                                    errorCode = 100;//başarılı
                                }
                            }
                            else
                            {
                                errorCode = 500;//kategori bulunamadı.
                            }
                        }
                        else
                        {
                            errorCode = 400;//görsel upload edilmeden kaydedilemez.
                        }
                    }
                    else
                    {
                        errorCode = 300;//görsel boş bırakılamaz
                    }
                }
                else
                {
                    errorCode = 200;//Name alanı boş olamaz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        public static int DeleteCategoryImageBeforeUpdate(string imagePath)
        {
            int errorCode = 0;
            try
            {
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("/UploadImages/TempCategory/" + imagePath)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath("/UploadImages/TempCategory/" + imagePath));
                }
                errorCode = 100;
            }
            catch (Exception)
            {
                errorCode = 99;
            }
            return errorCode;
        }

        #endregion

        #region CafeCommentList

        public static int DeleteCafeCommentControl(int ID)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                model.DELETECafeComment_WHEREID(ID);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return errorCode;
        }

        public static int ChangeCafeCommentIsNewControl(int ID)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                CafeComment cafeComment = model.SELECTCafeComment_WHEREID(ID);
                if (cafeComment.IsNew == true)
                {
                    cafeComment.IsNew = false;
                }
                else
                {
                    cafeComment.IsNew = true;
                }
                model.UPDATECafeComment_WHEREID(cafeComment);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region NewCafeComment

        public static int SaveCafeCommentControl(CafeComment cafeComment)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (cafeComment.CommentText != null && cafeComment.CommentText != "")
                {
                    if (cafeComment.Plus < 1000000000 && cafeComment.Plus >= 0)
                    {
                        if (cafeComment.Cons < 1000000000 && cafeComment.Cons >= 0)
                        {
                            cafeComment.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                            cafeComment.RecordDate = DateTime.Now;
                            model.INSERTCafeComment(cafeComment);
                            errorCode = 100;//başarılı
                        }
                        else
                        {
                            errorCode = 400;
                        }
                    }
                    else
                    {
                        errorCode = 300;
                    }
                }
                else
                {
                    errorCode = 200;//Yorum alanı boş olamaz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region EditCafeComment

        public static int UpdateCafeCommentControl(CafeComment cafeComment)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (cafeComment.CommentText != null && cafeComment.CommentText != "")
                {
                    if (cafeComment.Plus < 1000000000 && cafeComment.Plus >= 0)
                    {
                        if (cafeComment.Cons < 1000000000 && cafeComment.Cons >= 0)
                        {
                            cafeComment.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                            model.UPDATECafeComment_WHEREID(cafeComment);
                            errorCode = 100;//başarılı
                        }
                        else
                        {
                            errorCode = 400;
                        }
                    }
                    else
                    {
                        errorCode = 300;
                    }
                }
                else
                {
                    errorCode = 200;//Yorum alanı boş olamaz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region MenuCommentList

        public static int DeleteMenuCommentControl(int ID)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                model.DELETEMenuComment_WHEREID(ID);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return errorCode;
        }

        public static int ChangeMenuCommentIsNewControl(int ID)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                MenuComment menuComment = model.SELECTMenuComment_WHEREID(ID);
                if (menuComment.IsNew == true)
                {
                    menuComment.IsNew = false;
                }
                else
                {
                    menuComment.IsNew = true;
                }
                model.UPDATEMenuComment_WHEREID(menuComment);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region NewMenuComment

        public static int SaveMenuCommentControl(MenuComment menuComment, List<int> menuIDList)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (menuComment.CommentText != null && menuComment.CommentText != "")
                {
                    if (menuIDList != null && menuIDList.Count != 0)
                    {
                        if (menuComment.Plus < 1000000000 && menuComment.Plus >= 0)
                        {
                            if (menuComment.Cons < 1000000000 && menuComment.Cons >= 0)
                            {
                                menuComment.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                                foreach (var Menu_ID in menuIDList)
                                {
                                    Menu menu = new Menu();
                                    menu = model.SELECTMenu_WHEREID(Menu_ID);
                                    if (menu == null)
                                    {
                                        errorCode = 600;
                                        break;
                                    }
                                    else
                                    {
                                        menuComment.Menu_ID = Menu_ID;
                                        menuComment.RecordDate = DateTime.Now;
                                        model.INSERTMenuComment(menuComment);
                                    }
                                }
                                errorCode = 100;
                            }
                            else
                            {
                                errorCode = 500;
                            }
                        }
                        else
                        {
                            errorCode = 400;
                        }
                    }
                    else
                    {
                        errorCode = 300;
                    }
                }
                else
                {
                    errorCode = 200;//Yorum alanı boş olamaz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region EditMenuComment

        public static int UpdateMenuCommentControl(MenuComment menuComment)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (menuComment.CommentText != null && menuComment.CommentText != "")
                {
                    if (menuComment.Menu_ID != 0)
                    {
                        if (menuComment.Plus < 1000000000 && menuComment.Plus >= 0)
                        {
                            if (menuComment.Cons < 1000000000 && menuComment.Cons >= 0)
                            {

                                menuComment.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                                model.UPDATEMenuComment_WHEREID(menuComment);
                                errorCode = 100;
                            }
                            else
                            {
                                errorCode = 500;
                            }
                        }
                        else
                        {
                            errorCode = 400;
                        }
                    }
                    else
                    {
                        errorCode = 300;
                    }
                }
                else
                {
                    errorCode = 200;//Yorum alanı boş olamaz
                }
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region QrCodeList

        public static int DeleteQrCodeControl(int ID)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                model.DELETEQrCode_WHEREID(ID);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region NewQrCode

        public static int SaveQrCodeControl(QrCode qrCode)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (qrCode.TableNo != 0)
                {
                    qrCode.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                    QrCode qrCodeTableNoControl = model.SELECTQrCode_WHERETableNo(qrCode.TableNo, qrCode.User_ID);
                    if (qrCodeTableNoControl == null)
                    {
                        model.INSERTQrCode(qrCode);
                        errorCode = 100;//başarılı
                    }
                    else
                    {
                        errorCode = 300;
                    }
                }
                else
                {
                    errorCode = 200;
                }

            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region EditQrCode

        public static int UpdateQrCodeControl(QrCode qrCode)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                if (qrCode.TableNo != 0)
                {
                    qrCode.User_ID = (HttpContext.Current.Session["user"] as User).ID;
                    QrCode qrCodeTableNoControl = model.SELECTQrCode_WHERETableNo(qrCode.TableNo, qrCode.User_ID);
                    if (qrCodeTableNoControl == null)
                    {
                        model.UPDATEQrCode_WHEREID(qrCode);
                        errorCode = 100;//başarılı
                    }
                    else
                    {
                        if (qrCodeTableNoControl.TableNo == qrCode.TableNo)
                        {
                            model.UPDATEQrCode_WHEREID(qrCode);
                            errorCode = 100;//başarılı
                        }
                        else
                        {
                            errorCode = 300;
                        }
                    }

                }
                else
                {
                    errorCode = 200;
                }


            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99; //beklnmedik bir hata oluştu
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region ScanList

        public static int ChangeScanIsNewControl(int ID)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                Scan scan = model.SELECTScan_WHEREID(ID);
                if (scan.IsNew == true)
                {
                    scan.IsNew = false;
                }
                else
                {
                    scan.IsNew = true;
                }
                model.UPDATEScan_WHEREID(scan);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region CallList

        public static int ChangeCallIsNewControl(int ID)
        {
            int errorCode = 0;
            Model model = new Model();
            try
            {
                Call call = model.SELECTCall_WHEREID(ID);
                if (call.IsNew == true)
                {
                    call.IsNew = false;
                }
                else
                {
                    call.IsNew = true;
                }
                model.UPDATECall_WHEREID(call);
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return errorCode;
        }

        #endregion

        #region Assets

        public static string CalculateRelativeTime(DateTime dateTime)
        {
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var ts = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
            {
                return ts.Seconds == 1 ? "bir saniye önce" : ts.Seconds + " saniye önce";
            }
            if (delta < 2 * minute)
            {
                return "bir dakika önce";
            }
            if (delta < 45 * minute)
            {
                return ts.Minutes + " dakika önce";
            }
            if (delta < 90 * minute)
            {
                return "bir saat önce";
            }
            if (delta < 24 * hour)
            {
                return ts.Hours + " saat önce";
            }
            if (delta < 48 * hour)
            {
                return "dün";
            }
            if (delta < 30 * day)
            {
                return ts.Days + " gün önce";
            }
            if (delta < 12 * month)
            {
                var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "geçen ay" : months + " ay önce";
            }
            else
            {
                var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "geçen sene" : years + " sene önce";
            }
        }

        #endregion

        #region Global

        public static Tuple<int, List<int>> GetNotificationCountListControl()
        {
            int errorCode = 0;
            Model model = new Model();
            List<int> countList = new List<int>();
            try
            {
                int user_ID = (HttpContext.Current.Session["user"] as User).ID;
                Set set = model.SELECTSet(user_ID);

                if (set != null)
                {

                    if (set.ShowScanNotification)
                    {
                        int scanCount = model.COUNTScan_WHEREIsNew(user_ID, true);
                        countList.Add(scanCount);
                    }
                    else
                    {
                        countList.Add(-1);
                    }

                    if (set.ShowCallNotification)
                    {
                        int callCount = model.COUNTCall_WHEREIsNew(user_ID, true);
                        countList.Add(callCount);
                    }
                    else
                    {
                        countList.Add(-1);
                    }

                    if (set.ShowCafeCommentNotification)
                    {
                        int cafeCommentCount = model.COUNTCafeComment_WHEREIsNew(user_ID, true);
                        countList.Add(cafeCommentCount);
                    }
                    else
                    {
                        countList.Add(-1);
                    }

                    if (set.ShowMenuCommentNotification)
                    {
                        int menuCommentCount = model.COUNTMenuComment_WHEREIsNew(user_ID, true);
                        countList.Add(menuCommentCount);
                    }
                    else
                    {
                        countList.Add(-1);
                    }
                    errorCode = 100;
                }
                else
                {
                    errorCode = 200;
                }

            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, countList); ;
        }

        public static Tuple<int, List<Scan>> GetScanNotificationListControl()
        {
            int errorCode = 0;
            Model model = new Model();
            List<Scan> scanList = new List<Scan>();
            try
            {
                int user_ID = (HttpContext.Current.Session["user"] as User).ID;
                scanList = model.SELECTScan_WHEREIsNew_ORDERBYRecordDate(user_ID, true);
                for (int i = 0; i < scanList.Count; i++)
                {
                    scanList[i].HelperDateTimeRelative = CalculateRelativeTime(scanList[i].RecordDate);
                }
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, scanList);
        }

        public static Tuple<int, List<Call>> GetCallNotificationListControl()
        {
            int errorCode = 0;
            Model model = new Model();
            List<Call> callList = new List<Call>();
            try
            {
                int user_ID = (HttpContext.Current.Session["user"] as User).ID;
                callList = model.SELECTCall_WHEREIsNew_ORDERBYRecordDate(user_ID, true);
                for (int i = 0; i < callList.Count; i++)
                {
                    callList[i].HelperDateTimeRelative = CalculateRelativeTime(callList[i].RecordDate);
                }
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, callList);
        }

        public static Tuple<int, List<CafeComment>> GetCafeCommentNotificationListControl()
        {
            int errorCode = 0;
            Model model = new Model();
            List<CafeComment> cafeCommentList = new List<CafeComment>();
            try
            {
                int user_ID = (HttpContext.Current.Session["user"] as User).ID;
                cafeCommentList = model.SELECTCafeComment_WHEREIsNew_ORDERBYRecordDate(user_ID, true);
                for (int i = 0; i < cafeCommentList.Count; i++)
                {
                    if (cafeCommentList[i].CommentText.Length >= 10)
                    {
                        cafeCommentList[i].CommentText = cafeCommentList[i].CommentText.Substring(0, 9) + "...";
                    }
                    cafeCommentList[i].HelperDateTimeRelative = CalculateRelativeTime(cafeCommentList[i].RecordDate);
                }
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, cafeCommentList);
        }

        public static Tuple<int, List<MenuComment>> GetMenuCommentNotificationListControl()
        {
            int errorCode = 0;
            Model model = new Model();
            List<MenuComment> menuCommentList = new List<MenuComment>();
            try
            {
                int user_ID = (HttpContext.Current.Session["user"] as User).ID;
                menuCommentList = model.SELECTMenuComment_WHEREIsNew_ORDERBYRecordDate(user_ID, true);
                for (int i = 0; i < menuCommentList.Count; i++)
                {
                    if (menuCommentList[i].CommentText.Length >= 10)
                    {
                        menuCommentList[i].CommentText = menuCommentList[i].CommentText.Substring(0, 9) + "...";
                    }

                    menuCommentList[i].HelperDateTimeRelative = CalculateRelativeTime(menuCommentList[i].RecordDate);
                    menuCommentList[i].HelperMenuName = model.SELECTMenuCOLUMNName_WHEREID(menuCommentList[i].Menu_ID);

                    if (menuCommentList[i].HelperMenuName.Length >= 10)
                    {
                        menuCommentList[i].HelperMenuName = menuCommentList[i].HelperMenuName.Substring(0, 9) + "...";
                    }
                }
                errorCode = 100;
            }
            catch (Exception)
            {
                model.Close();
                errorCode = 99;
            }
            model.Close();
            return Tuple.Create(errorCode, menuCommentList);
        }

        #endregion




    }
}