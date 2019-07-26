using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using netgarson.Entities;
using netgarson.Helpers;
using netgarson.Models;

namespace netgarson.Controllers
{
    public class AdminController : Controller
    {

        #region Login

        public ActionResult Login()
        {
            try
            {
                if (Request.Cookies["loginValues"] != null)
                {
                    string mail = Request.Cookies["loginValues"].Values["mail"];
                    string password = Request.Cookies["loginValues"].Values["password"];
                    int errorCode = InputControl.LoginUserControl(mail, password, true);
                    if (errorCode == 100)
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Admin");
            }

        }

        [HttpPost]
        public ActionResult LoginUser(string mail, string password, bool rememberMe)
        {
            int errorCode = InputControl.LoginUserControl(mail, password, rememberMe);
            return Json(new { result = errorCode });
        }

        [HttpPost]
        public ActionResult SendRepasswordMail(string mail)
        {
            int errorCode = InputControl.SendRepasswordMailControl(mail);
            return Json(new { result = errorCode });
        }

        #endregion

        #region Index
        [LoginAuthentication(ViewName = "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDashboardTotalCountList()
        {
            var tupleResult = InputControl.GetDashboardTotalCountListControl();
            int errorCode = tupleResult.Item1;
            List<int> countList = tupleResult.Item2;
            return Json(new { result = errorCode, countList });
        }

        [HttpPost]
        public ActionResult GetDashboardChart()
        {
            var tupleResult = InputControl.GetDashboardChartControl();
            int errorCode = tupleResult.Item1;
            Set set = tupleResult.Item2;
            return Json(new { result = errorCode, set });
        }

        #endregion

        #region MenuList

        [LoginAuthentication(ViewName = "MenuList")]
        public ActionResult MenuList()
        {
            Model model = new Model();
            try
            {
                List<Menu> menuList = model.SELECTMenu((Session["User"] as User).ID);
                model.Close();
                return View(menuList);
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }

        }

        [LoginAuthentication]
        public ActionResult DeleteMenu(int ID)
        {
            int errorCode = InputControl.DeleteMenuControl(ID);
            return Json(new { result = errorCode });
        }

        #endregion

        #region NewMenu

        [LoginAuthentication(ViewName = "NewMenu")]
        public ActionResult NewMenu()
        {
            Model model = new Model();
            try
            {
                List<Category> categoryList = model.SELECTCategory((Session["User"] as User).ID);
                model.Close();
                return View(categoryList);
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult SaveMenu(Menu menu, List<CategoryMenuRel> categoryMenuRelList)
        {
            int errorCode = InputControl.SaveMenuControl(menu, categoryMenuRelList);
            return Json(new { result = errorCode });
        }

        #endregion

        #region EditMenu

        [LoginAuthentication]
        public ActionResult EditMenu(int ID)
        {
            Model model = new Model();
            try
            {
                Menu menu = model.SELECTMenu_WHEREID(ID);
                if (menu != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("/UploadImages/Menu/" + menu.ImagePath)))
                    {
                        if (!System.IO.File.Exists(Server.MapPath("/UploadImages/TempMenu/" + menu.ImagePath)))
                        {
                            System.IO.File.Copy(Server.MapPath("/UploadImages/Menu/" + menu.ImagePath), Server.MapPath("/UploadImages/TempMenu/" + menu.ImagePath));
                        }
                    }

                    List<Category> categoryList = model.SELECTCategory((Session["User"] as User).ID);
                    List<CategoryMenuRel> categoryMenuRelList = model.SELECTCategoryMenuRel_WHEREMenu_ID(ID);
                    model.Close();
                    return View(Tuple.Create(menu, categoryList, categoryMenuRelList));
                }
                else
                {
                    model.Close();
                    return RedirectToAction("Error", "Admin");
                }
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }
        }

        [LoginAuthentication]
        public ActionResult DeleteMenuImageBeforeUpdate(string imagePath)
        {
            int errorCode = InputControl.DeleteMenuImageBeforeUpdate(imagePath);
            return Json(new { result = errorCode });
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult UpdateMenu(Menu menu, List<CategoryMenuRel> categoryMenuRelList)
        {
            int errorCode = InputControl.UpdateMenuControl(menu, categoryMenuRelList);
            return Json(new { result = errorCode });
        }

        #endregion

        #region CategoryList

        [LoginAuthentication(ViewName = "CategoryList")]
        public ActionResult CategoryList()
        {
            Model model = new Model();
            try
            {
                List<Category> categoryList = model.SELECTCategory((Session["User"] as User).ID);
                model.Close();
                return View(categoryList);
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }
        }

        [LoginAuthentication]
        public ActionResult AjaxGetMenuFromCategory(int category_ID)
        {
            var tupleResult = InputControl.AjaxGetMenuFromCategoryControl(category_ID);
            int errorCode = tupleResult.Item1;
            List<Menu> menuList = tupleResult.Item2;
            return Json(new { result = errorCode, menuList });
        }

        [LoginAuthentication]
        public ActionResult DeleteCategory(int category_ID)
        {
            int errorCode = InputControl.DeleteCategoryControl(category_ID);
            return Json(new { result = errorCode });
        }

        #endregion

        #region NewCategory

        [LoginAuthentication(ViewName = "NewCategory")]
        public ActionResult NewCategory()
        {
            return View();
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult SaveCategory(Category category)
        {
            int errorCode = InputControl.SaveCategoryControl(category);
            return Json(new { result = errorCode });
        }

        #endregion

        #region EditCategory

        [LoginAuthentication]
        public ActionResult EditCategory(int ID)
        {
            Model model = new Model();
            try
            {
                Category category = model.SELECTCategory_WHEREID(ID);
                if (category != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("/UploadImages/Category/" + category.ImagePath)))
                    {
                        if (!System.IO.File.Exists(Server.MapPath("/UploadImages/TempCategory/" + category.ImagePath)))
                        {
                            System.IO.File.Copy(Server.MapPath("/UploadImages/Category/" + category.ImagePath), Server.MapPath("/UploadImages/TempCategory/" + category.ImagePath));
                        }
                    }
                    model.Close();
                    return View(category);
                }
                else
                {
                    model.Close();
                    return RedirectToAction("Error", "Admin");
                }
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }
        }

        [LoginAuthentication]
        public ActionResult DeleteCategoryImageBeforeUpdate(string imagePath)
        {
            int errorCode = InputControl.DeleteCategoryImageBeforeUpdate(imagePath);
            return Json(new { result = errorCode });
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult UpdateCategory(Category category)
        {
            int errorCode = InputControl.UpdateCategoryControl(category);
            return Json(new { result = errorCode });
        }

        #endregion

        #region CafeCommentList

        [LoginAuthentication(ViewName = "CafeCommentList")]
        public ActionResult CafeCommentList()
        {
            Model model = new Model();
            try
            {
                List<CafeComment> cafeCommentList = model.SELECTCafeComment((Session["User"] as User).ID);
                model.Close();
                return View(cafeCommentList);
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }

        }

        [LoginAuthentication]
        public ActionResult DeleteCafeComment(int ID)
        {
            int errorCode = InputControl.DeleteCafeCommentControl(ID);
            return Json(new { result = errorCode });
        }

        [LoginAuthentication]
        public ActionResult ChangeCafeCommentIsNew(int ID)
        {
            int errorCode = InputControl.ChangeCafeCommentIsNewControl(ID);
            return Json(new { result = errorCode });
        }

        #endregion

        #region NewCafeComment

        [LoginAuthentication(ViewName = "NewCafeComment")]
        public ActionResult NewCafeComment()
        {
            return View();
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult SaveCafeComment(CafeComment cafeComment)
        {
            int errorCode = InputControl.SaveCafeCommentControl(cafeComment);
            return Json(new { result = errorCode });
        }

        #endregion

        #region EditCafeComment

        [LoginAuthentication]
        public ActionResult EditCafeComment(int ID)
        {
            Model model = new Model();
            try
            {
                CafeComment cafeComment = model.SELECTCafeComment_WHEREID(ID);
                if (cafeComment != null)
                {
                    return View(cafeComment);
                }
                else
                {
                    model.Close();
                    return RedirectToAction("Error", "Admin");
                }
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult UpdateCafeComment(CafeComment cafeComment)
        {
            int errorCode = InputControl.UpdateCafeCommentControl(cafeComment);
            return Json(new { result = errorCode });
        }

        #endregion

        #region MenuCommentList

        [LoginAuthentication(ViewName = "MenuCommentList")]
        public ActionResult MenuCommentList()
        {
            Model model = new Model();
            try
            {
                List<MenuComment> menuCommentList = model.SELECTMenuComment((Session["User"] as User).ID);
                for (int i = 0; i < menuCommentList.Count; i++)
                {
                    menuCommentList[i].HelperMenuName = model.SELECTMenuCOLUMNName_WHEREID(menuCommentList[i].Menu_ID);
                }
                model.Close();
                return View(menuCommentList);
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }

        }

        [LoginAuthentication]
        public ActionResult DeleteMenuComment(int ID)
        {
            int errorCode = InputControl.DeleteMenuCommentControl(ID);
            return Json(new { result = errorCode });
        }

        [LoginAuthentication]
        public ActionResult ChangeMenuCommentIsNew(int ID)
        {
            int errorCode = InputControl.ChangeMenuCommentIsNewControl(ID);
            return Json(new { result = errorCode });
        }

        #endregion

        #region NewMenuComment

        [LoginAuthentication(ViewName = "NewMenuComment")]
        public ActionResult NewMenuComment()
        {
            Model model = new Model();
            try
            {
                List<Menu> menuList = model.SELECTMenu((Session["User"] as User).ID);
                model.Close();
                return View(menuList);
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult SaveMenuComment(MenuComment menuComment, List<int> menuIDList)
        {
            int errorCode = InputControl.SaveMenuCommentControl(menuComment, menuIDList);
            return Json(new { result = errorCode });
        }

        #endregion

        #region EditMenuComment

        [LoginAuthentication]
        public ActionResult EditMenuComment(int ID)
        {
            Model model = new Model();
            try
            {
                MenuComment menuComment = model.SELECTMenuComment_WHEREID(ID);
                if (menuComment != null)
                {
                    List<Menu> menuList = model.SELECTMenu((Session["User"] as User).ID);
                    //List<MenuComment> menuCommentList = model.SELECTMenuComment_WHERECommentText(menuComment.CommentText, (Session["User"] as User).ID);
                    model.Close();
                    //return View(Tuple.Create(menuComment, menuList, menuCommentList));
                    return View(Tuple.Create(menuComment, menuList));
                }
                else
                {
                    model.Close();
                    return RedirectToAction("Error", "Admin");
                }
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult UpdateMenuComment(MenuComment menuComment)
        {
            int errorCode = InputControl.UpdateMenuCommentControl(menuComment);
            return Json(new { result = errorCode });
        }

        #endregion

        #region QrCodeList

        [LoginAuthentication(ViewName = "CafeCommentList")]
        public ActionResult QrCodeList()
        {
            Model model = new Model();
            try
            {
                List<QrCode> qrCodeList = model.SELECTQrCode((Session["User"] as User).ID);
                model.Close();
                return View(qrCodeList);
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }

        }

        [LoginAuthentication]
        public ActionResult DeleteQrCode(int ID)
        {
            int errorCode = InputControl.DeleteQrCodeControl(ID);
            return Json(new { result = errorCode });
        }

        #endregion

        #region NewQrCode

        [LoginAuthentication(ViewName = "NewQrCode")]
        public ActionResult NewQrCode()
        {
            return View();
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult SaveQrCode(QrCode qrCode)
        {
            int errorCode = InputControl.SaveQrCodeControl(qrCode);
            return Json(new { result = errorCode });
        }

        #endregion

        #region EditQrCode

        [LoginAuthentication]
        public ActionResult EditQrCode(int ID)
        {
            Model model = new Model();
            try
            {
                QrCode qrCode = model.SELECTQrCode_WHEREID(ID);
                if (qrCode != null)
                {
                    return View(qrCode);
                }
                else
                {
                    model.Close();
                    return RedirectToAction("Error", "Admin");
                }
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult UpdateQrCode(QrCode qrCode)
        {
            int errorCode = InputControl.UpdateQrCodeControl(qrCode);
            return Json(new { result = errorCode });
        }

        #endregion

        #region CallList

        [LoginAuthentication(ViewName = "CallList")]
        public ActionResult CallList()
        {
            Model model = new Model();
            try
            {
                List<Call> callList = model.SELECTCall((Session["User"] as User).ID);
                model.Close();
                return View(callList);
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }

        }

        [LoginAuthentication]
        public ActionResult ChangeCallIsNew(int ID)
        {
            int errorCode = InputControl.ChangeCallIsNewControl(ID);
            return Json(new { result = errorCode });
        }

        #endregion

        #region ScanList

        [LoginAuthentication(ViewName = "ScanList")]
        public ActionResult ScanList()
        {
            Model model = new Model();
            try
            {
                List<Scan> scanList = model.SELECTScan((Session["User"] as User).ID);
                model.Close();
                return View(scanList);
            }
            catch (Exception)
            {
                model.Close();
                return RedirectToAction("Error", "Admin");
            }

        }

        [LoginAuthentication]
        public ActionResult ChangeScanIsNew(int ID)
        {
            int errorCode = InputControl.ChangeScanIsNewControl(ID);
            return Json(new { result = errorCode });
        }

        #endregion

        #region Assets

        [HttpPost]
        [LoginAuthentication]
        public ActionResult UploadMenuImage()
        {
            int errorCode = 0;
            try
            {
                HttpPostedFileBase hpf = HttpContext.Request.Files["file"] as HttpPostedFileBase;
                string tag = HttpContext.Request.Params["tags"];// The same param name that you put in extrahtml if you have some.
                string menu = HttpContext.Request.Params["menu"];
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath("~/UploadImages/TempMenu"));// If you don't have the folder yet, you need to create.
                Guid guid = Guid.NewGuid();
                string sentFileName = guid.ToString() + Path.GetFileName(hpf.FileName); //it can be just a file name or a user local path! it depends on the used browser. So we need to ensure that this var will contain just the file name.

                string savedFileName = Path.Combine(di.FullName, sentFileName);
                hpf.SaveAs(savedFileName);
                errorCode = 100;
                var msg = new { errorCode, filenameDB = sentFileName, filename = hpf.FileName, url = savedFileName };
                return Json(msg);
            }
            catch (Exception e)
            {
                errorCode = 99;
                var msg = new { errorCode };
                return Json(msg);
            }
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult DeleteMenuImage(string url)
        {
            int errorCode = 0;
            try
            {
                System.IO.File.Delete(url);
                errorCode = 100;
                var msg = new { errorCode };
                return Json(msg);
            }
            catch (Exception e)
            {
                errorCode = 99;
                var msg = new { errorCode };
                return Json(msg);
            }
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult UploadCategoryImage()
        {
            int errorCode = 0;
            try
            {
                HttpPostedFileBase hpf = HttpContext.Request.Files["file"] as HttpPostedFileBase;
                string tag = HttpContext.Request.Params["tags"];// The same param name that you put in extrahtml if you have some.
                string category = HttpContext.Request.Params["category"];
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath("~/UploadImages/TempCategory"));// If you don't have the folder yet, you need to create.
                Guid guid = Guid.NewGuid();
                string sentFileName = guid.ToString() + Path.GetFileName(hpf.FileName); //it can be just a file name or a user local path! it depends on the used browser. So we need to ensure that this var will contain just the file name.

                string savedFileName = Path.Combine(di.FullName, sentFileName);
                hpf.SaveAs(savedFileName);
                errorCode = 100;
                var msg = new { errorCode, filenameDB = sentFileName, filename = hpf.FileName, url = savedFileName };
                return Json(msg);
            }
            catch (Exception e)
            {
                errorCode = 99;
                var msg = new { errorCode };
                return Json(msg);
            }
        }

        [HttpPost]
        [LoginAuthentication]
        public ActionResult DeleteCategoryImage(string url)
        {
            int errorCode = 0;
            try
            {
                System.IO.File.Delete(url);
                errorCode = 100;
                var msg = new { errorCode };
                return Json(msg);
            }
            catch (Exception e)
            {
                //If you want this working with a custom error you need to change the name of 
                //variable customErrorKeyStr in line 85, from jquery-upload-file-error to jquery_upload_file_error 
                errorCode = 99;
                var msg = new { errorCode };
                return Json(msg);
            }
        }

        #endregion

        #region Global

        public ActionResult LoginAuthenticationRouter(string view)
        {
            try
            {
                if (Request.Cookies["loginValues"] != null)
                {
                    string mail = Request.Cookies["loginValues"].Values["mail"];
                    string password = Request.Cookies["loginValues"].Values["password"];
                    int errorCode = InputControl.LoginUserControl(mail, password);
                    if (errorCode == 100)
                    {
                        return RedirectToAction(view, "Admin");
                    }
                }
                return RedirectToAction("Login", "Admin");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Admin");
            }

        }

        [HttpPost]
        public ActionResult Logout()
        {
            int errorCode = InputControl.LogoutControl();
            return Json(new { result = errorCode });

        }

        [HttpPost]
        public ActionResult GetNotificationCountList()
        {
            var tupleResult = InputControl.GetNotificationCountListControl();
            int errorCode = tupleResult.Item1;
            List<int> countList = tupleResult.Item2;
            return Json(new { result = errorCode, countList });
        }

        [HttpPost]
        public ActionResult GetScanNotificationList()
        {
            var tupleResult = InputControl.GetScanNotificationListControl();
            int errorCode = tupleResult.Item1;
            List<Scan> scanList = tupleResult.Item2;
            return Json(new { result = errorCode, scanList });
        }

        [HttpPost]
        public ActionResult GetCallNotificationList()
        {
            var tupleResult = InputControl.GetCallNotificationListControl();
            int errorCode = tupleResult.Item1;
            List<Call> callList = tupleResult.Item2;
            return Json(new { result = errorCode, callList });
        }

        [HttpPost]
        public ActionResult GetCafeCommentNotificationList()
        {
            var tupleResult = InputControl.GetCafeCommentNotificationListControl();
            int errorCode = tupleResult.Item1;
            List<CafeComment> cafeCommentList = tupleResult.Item2;
            return Json(new { result = errorCode, cafeCommentList });
        }

        [HttpPost]
        public ActionResult GetMenuCommentNotificationList()
        {
            var tupleResult = InputControl.GetMenuCommentNotificationListControl();
            int errorCode = tupleResult.Item1;
            List<MenuComment> menuCommentList = tupleResult.Item2;
            return Json(new { result = errorCode, menuCommentList });
        }

        #endregion

    }
}