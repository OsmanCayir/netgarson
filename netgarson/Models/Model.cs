using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using netgarson.Entities;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace netgarson.Models
{
    public class Model
    {
        public static SqlConnection connection;

        public Model()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
        }

        #region Category

        public void DELETECategory_WHEREID(int ID)
        {
            connection.Execute("DELETE FROM netgarson.dbo.[Category] WHERE ID=@ID", new { ID });
        }

        public Category SELECTCategory_WHERENameANDUser_ID(string name, int user_ID)
        {
            return connection.Query<Category>("SELECT * FROM netgarson.dbo.[Category] WHERE Name = @name AND User_ID = @user_ID", new { name, user_ID }).FirstOrDefault();
        }

        public void INSERTCategory(Category category)
        {
            connection.Execute("INSERT INTO netgarson.dbo.[Category]([Name], [Description], [ImagePath], [ShowComment], [Active], [User_ID]) VALUES (@Name, @Description, @ImagePath, @ShowComment, @Active, @User_ID)", new { category.Name, category.Description, category.ImagePath, category.ShowComment, category.Active, category.User_ID });
        }

        public void UPDATECategory_WHEREID(Category category)
        {
            connection.Execute("UPDATE netgarson.dbo.[Category] SET Name = @Name, Description = @Description, ImagePath = @ImagePath, ShowComment = @ShowComment, Active = @Active, User_ID = @User_ID WHERE ID = @ID", new { category.Name, category.Description, category.ImagePath, category.ShowComment, category.Active, category.User_ID, category.ID });
        }

        public List<Category> SELECTCategory(int user_ID)//
        {
            return connection.Query<Category>("SELECT * FROM netgarson.dbo.[Category] WHERE User_ID = @user_ID", new { user_ID }).ToList();
        }

        public Category SELECTCategory_WHEREID(int ID)//
        {
            return connection.Query<Category>("SELECT * FROM netgarson.dbo.[Category] WHERE ID = @ID", new { ID }).FirstOrDefault();
        }

        #endregion

        #region CategoryMenuRel

        public void INSERTCategoryMenuRel(CategoryMenuRel categoryMenuRel)
        {
            connection.Execute("INSERT INTO netgarson.dbo.[CategoryMenuRel]([Category_ID], [Menu_ID]) VALUES (@Category_ID, @Menu_ID)", new { categoryMenuRel.Category_ID, categoryMenuRel.Menu_ID });
        }

        public void DELETECategoryMenuRel_WHERECategory_ID(int category_ID)
        {
            connection.Execute("DELETE FROM netgarson.dbo.[CategoryMenuRel] WHERE Category_ID=@Category_ID", new { category_ID });
        }

        public void DELETECategoryMenuRel_WHEREMenu_ID(int menu_ID)
        {
            connection.Execute("DELETE FROM netgarson.dbo.[CategoryMenuRel] WHERE Menu_ID=@Menu_ID", new { menu_ID });
        }

        public List<CategoryMenuRel> SELECTCategoryMenuRel_WHEREMenu_ID(int menu_ID)//
        {
            return connection.Query<CategoryMenuRel>("SELECT * FROM netgarson.dbo.[CategoryMenuRel] WHERE Menu_ID = @Menu_ID", new { menu_ID }).ToList();
        }

        #endregion

        #region MailQueue

        public void INSERTMailQueue(MailQueue mailQueue)
        {
            connection.Execute("INSERT INTO netgarson.dbo.[MailQueue]([Mail], [MailState], [MailStateDescription], [ContentType], [ContentTypeDescription], [RecordDate], [SendDate]) VALUES (@Mail, @MailState, @MailStateDescription, @ContentType, @ContentTypeDescription, @RecordDate, @SendDate)", new { mailQueue.Mail, mailQueue.MailState, mailQueue.MailStateDescription, mailQueue.ContentType, mailQueue.ContentTypeDescription, mailQueue.RecordDate, mailQueue.SendDate });
        }

        #endregion

        #region Menu

        public List<Menu> SELECTMenu_WHERECategory_ID(int category_ID)
        {
            return connection.Query<Menu>("Select netgarson.dbo.[Menu].* FROM netgarson.dbo.[Menu] LEFT JOIN netgarson.dbo.[CategoryMenuRel] ON netgarson.dbo.[Menu].ID = netgarson.dbo.[CategoryMenuRel].Menu_ID WHERE netgarson.dbo.[CategoryMenuRel].Category_ID = @Category_ID", new { category_ID }).ToList();
        }

        public List<Menu> SELECTMenu(int user_ID)
        {
            return connection.Query<Menu>("SELECT * FROM netgarson.dbo.[Menu] WHERE User_ID = @user_ID", new { user_ID }).ToList();
        }

        public void INSERTMenu(Menu menu)
        {
            connection.Execute("INSERT INTO netgarson.dbo.[Menu]([Name], [Description], [Price], [ImagePath], [ShowComment], [Active], [User_ID]) VALUES (@Name, @Description, @Price, @ImagePath, @ShowComment, @Active, @User_ID)", new { menu.Name, menu.Description, menu.Price, menu.ImagePath, menu.ShowComment, menu.Active, menu.User_ID });
        }

        public void UPDATEMenu_WHEREID(Menu menu)
        {
            connection.Execute("UPDATE netgarson.dbo.[Menu] SET Name = @Name, Description = @Description, Price = @Price, ImagePath = @ImagePath, ShowComment = @ShowComment, Active = @Active, User_ID = @User_ID WHERE ID = @ID", new { menu.Name, menu.Description, menu.Price, menu.ImagePath, menu.ShowComment, menu.Active, menu.User_ID, menu.ID });
        }

        public Menu SELECTMenu_WHEREID(int ID)//
        {
            return connection.Query<Menu>("SELECT * FROM netgarson.dbo.[Menu] WHERE ID = @ID", new { ID }).FirstOrDefault();
        }

        public Menu SELECTMenu_WHERENameANDUser_ID(string name, int user_ID)
        {
            return connection.Query<Menu>("SELECT * FROM netgarson.dbo.[Menu] WHERE Name = @name AND User_ID = @user_ID", new { name, user_ID }).FirstOrDefault();
        }

        public int INSERTMenu_RETURNID(Menu menu)
        {
            return connection.Query<int>("INSERT INTO netgarson.dbo.[Menu]([Name], [Description], [Price], [ImagePath], [ShowComment], [Active], [User_ID]) VALUES (@Name, @Description, @Price, @ImagePath, @ShowComment, @Active, @User_ID);SELECT CAST(SCOPE_IDENTITY() AS INT);", new { menu.Name, menu.Description, menu.Price, menu.ImagePath, menu.ShowComment, menu.Active, menu.User_ID }).Single();
        }

        public void DELETEMenu_WHEREID(int ID)
        {
            connection.Execute("DELETE FROM netgarson.dbo.[Menu] WHERE ID=@ID", new { ID });
        }

        public string SELECTMenuCOLUMNName_WHEREID(int ID)//
        {
            return connection.Query<string>("SELECT Name FROM netgarson.dbo.[Menu] WHERE ID = @ID", new { ID }).FirstOrDefault();
        }

        #endregion

        #region User

        public User SELECTUser_WHEREMailANDPassword(string mail, string password)
        {
            return connection.Query<User>("SELECT * FROM netgarson.dbo.[User] WHERE Mail = @Mail AND Password = @Password", new { mail, password }).FirstOrDefault();
        }

        public User SELECTUser_WHEREMail(string mail)
        {
            return connection.Query<User>("SELECT * FROM netgarson.dbo.[User] WHERE Mail = @Mail", new { mail }).FirstOrDefault();
        }

        #endregion

        #region CafeComment

        public List<CafeComment> SELECTCafeComment(int user_ID)
        {
            return connection.Query<CafeComment>("SELECT * FROM netgarson.dbo.[CafeComment] WHERE User_ID = @user_ID", new { user_ID }).ToList();
        }

        public void DELETECafeComment_WHEREID(int ID)
        {
            connection.Execute("DELETE FROM netgarson.dbo.[CafeComment] WHERE ID=@ID", new { ID });
        }

        public void INSERTCafeComment(CafeComment cafeComment)
        {
            connection.Execute("INSERT INTO netgarson.dbo.[CafeComment]([CommentText], [Plus], [Cons], [RecordDate], [IsNew], [Active], [User_ID]) VALUES (@CommentText, @Plus, @Cons, @RecordDate, @IsNew, @Active, @User_ID)", new { cafeComment.CommentText, cafeComment.Plus, cafeComment.Cons, cafeComment.RecordDate, cafeComment.IsNew, cafeComment.Active, cafeComment.User_ID });
        }

        public CafeComment SELECTCafeComment_WHEREID(int ID)//
        {
            return connection.Query<CafeComment>("SELECT * FROM netgarson.dbo.[CafeComment] WHERE ID = @ID", new { ID }).FirstOrDefault();
        }

        public void UPDATECafeComment_WHEREID(CafeComment cafeComment)
        {
            connection.Execute("UPDATE netgarson.dbo.[CafeComment] SET CommentText = @CommentText, Plus = @Plus, Cons = @Cons, IsNew = @IsNew, Active = @Active, User_ID = @User_ID WHERE ID = @ID", new { cafeComment.CommentText, cafeComment.Plus, cafeComment.Cons, cafeComment.IsNew, cafeComment.Active, cafeComment.User_ID, cafeComment.ID });
        }

        public int COUNTCafeComment(int user_ID)
        {
            return connection.Query<int>("SELECT COUNT(*) FROM netgarson.dbo.[CafeComment] WHERE User_ID = @user_ID", new { user_ID }).FirstOrDefault();
        }

        public int COUNTCafeComment_WHEREIsNew(int user_ID, bool isNew)
        {
            return connection.Query<int>("SELECT COUNT(*) FROM netgarson.dbo.[CafeComment] WHERE User_ID = @user_ID AND IsNew = @isNew", new { user_ID, isNew }).FirstOrDefault();
        }

        public List<CafeComment> SELECTCafeComment_WHEREIsNew_ORDERBYRecordDate(int user_ID, bool isNew)
        {
            return connection.Query<CafeComment>("SELECT * FROM netgarson.dbo.[CafeComment] WHERE User_ID = @user_ID AND IsNew = @isNew ORDER BY RecordDate DESC", new { user_ID, isNew }).ToList();
        }

        #endregion

        #region MenuComment

        public List<MenuComment> SELECTMenuComment(int user_ID)
        {
            return connection.Query<MenuComment>("SELECT * FROM netgarson.dbo.[MenuComment] WHERE User_ID = @user_ID", new { user_ID }).ToList();
        }

        public void DELETEMenuComment_WHEREID(int ID)
        {
            connection.Execute("DELETE FROM netgarson.dbo.[MenuComment] WHERE ID=@ID", new { ID });
        }

        public void INSERTMenuComment(MenuComment menuComment)
        {
            connection.Execute("INSERT INTO netgarson.dbo.[MenuComment]([CommentText], [Plus], [Cons], [RecordDate], [IsNew], [Active], [Menu_ID], [User_ID]) VALUES (@CommentText, @Plus, @Cons, @RecordDate, @IsNew, @Active, @Menu_ID, @User_ID)", new { menuComment.CommentText, menuComment.Plus, menuComment.Cons, menuComment.RecordDate, menuComment.IsNew, menuComment.Active, menuComment.Menu_ID, menuComment.User_ID });
        }

        public List<MenuComment> SELECTMenuComment_WHERECommentText(string commentText, int user_ID)
        {
            return connection.Query<MenuComment>("SELECT * FROM netgarson.dbo.[MenuComment] WHERE CommentText = @CommentText AND User_ID = @user_ID", new { commentText, user_ID }).ToList();
        }

        public MenuComment SELECTMenuComment_WHEREID(int ID)//
        {
            return connection.Query<MenuComment>("SELECT * FROM netgarson.dbo.[MenuComment] WHERE ID = @ID", new { ID }).FirstOrDefault();
        }

        public void UPDATEMenuComment_WHEREID(MenuComment menuComment)
        {
            connection.Execute("UPDATE netgarson.dbo.[MenuComment] SET CommentText = @CommentText, Plus = @Plus, Cons = @Cons, IsNew = @IsNew, Active = @Active, Menu_ID = @Menu_ID, User_ID = @User_ID WHERE ID = @ID", new { menuComment.CommentText, menuComment.Plus, menuComment.Cons, menuComment.IsNew, menuComment.Active, menuComment.Menu_ID, menuComment.User_ID, menuComment.ID });
        }

        public int COUNTMenuComment(int user_ID)
        {
            return connection.Query<int>("SELECT COUNT(*) FROM netgarson.dbo.[MenuComment] WHERE User_ID = @user_ID", new { user_ID }).FirstOrDefault();
        }

        public int COUNTMenuComment_WHEREIsNew(int user_ID, bool isNew)
        {
            return connection.Query<int>("SELECT COUNT(*) FROM netgarson.dbo.[MenuComment] WHERE User_ID = @user_ID AND IsNew = @isNew", new { user_ID, isNew }).FirstOrDefault();
        }

        public List<MenuComment> SELECTMenuComment_WHEREIsNew_ORDERBYRecordDate(int user_ID, bool isNew)
        {
            return connection.Query<MenuComment>("SELECT * FROM netgarson.dbo.[MenuComment] WHERE User_ID = @user_ID AND IsNew = @isNew ORDER BY RecordDate DESC", new { user_ID, isNew }).ToList();
        }
        #endregion

        #region QrCode

        public List<QrCode> SELECTQrCode(int user_ID)
        {
            return connection.Query<QrCode>("SELECT * FROM netgarson.dbo.[QrCode] WHERE User_ID = @user_ID", new { user_ID }).ToList();
        }

        public void DELETEQrCode_WHEREID(int ID)
        {
            connection.Execute("DELETE FROM netgarson.dbo.[QrCode] WHERE ID=@ID", new { ID });
        }

        public void INSERTQrCode(QrCode qrCode)
        {
            connection.Execute("INSERT INTO netgarson.dbo.[QrCode]([TableNo], [ScanCount], [CallCount], [Active], [User_ID]) VALUES (@TableNo, @ScanCount, @CallCount, @Active, @User_ID)", new { qrCode.TableNo, qrCode.ScanCount, qrCode.CallCount, qrCode.Active, qrCode.User_ID });
        }

        public QrCode SELECTQrCode_WHEREID(int ID)//
        {
            return connection.Query<QrCode>("SELECT * FROM netgarson.dbo.[QrCode] WHERE ID = @ID", new { ID }).FirstOrDefault();
        }

        public void UPDATEQrCode_WHEREID(QrCode qrCode)
        {
            connection.Execute("UPDATE netgarson.dbo.[QrCode] SET TableNo = @TableNo, ScanCount = @ScanCount, CallCount = @CallCount, Active = @Active, User_ID = @User_ID WHERE ID = @ID", new { qrCode.TableNo, qrCode.ScanCount, qrCode.CallCount, qrCode.Active, qrCode.User_ID, qrCode.ID });
        }

        public QrCode SELECTQrCode_WHERETableNo(int tableNo, int user_ID)//
        {
            return connection.Query<QrCode>("SELECT * FROM netgarson.dbo.[QrCode] WHERE TableNo = @TableNo AND User_ID = @user_ID", new { tableNo, user_ID }).FirstOrDefault();
        }

        #endregion

        #region Call

        public int COUNTCall(int user_ID)
        {
            return connection.Query<int>("SELECT COUNT(*) FROM netgarson.dbo.[Call] WHERE User_ID = @user_ID", new { user_ID }).FirstOrDefault();
        }

        public int COUNTCall_WHEREIsNew(int user_ID, bool isNew)
        {
            return connection.Query<int>("SELECT COUNT(*) FROM netgarson.dbo.[Call] WHERE User_ID = @user_ID AND IsNew = @isNew", new { user_ID, isNew }).FirstOrDefault();
        }

        public List<Call> SELECTCall(int user_ID)
        {
            return connection.Query<Call>("SELECT * FROM netgarson.dbo.[Call] WHERE User_ID = @user_ID", new { user_ID }).ToList();
        }

        public Call SELECTCall_WHEREID(int ID)//
        {
            return connection.Query<Call>("SELECT * FROM netgarson.dbo.[Call] WHERE ID = @ID", new { ID }).FirstOrDefault();
        }

        public void UPDATECall_WHEREID(Call call)
        {
            connection.Execute("UPDATE netgarson.dbo.[Call] SET TableNo = @TableNo, IsNew = @IsNew, User_ID = @User_ID WHERE ID = @ID", new { call.TableNo, call.IsNew, call.User_ID, call.ID });
        }

        public List<Call> SELECTCall_WHEREIsNew_ORDERBYRecordDate(int user_ID, bool isNew)
        {
            return connection.Query<Call>("SELECT * FROM netgarson.dbo.[Call] WHERE User_ID = @user_ID AND IsNew = @isNew ORDER BY RecordDate DESC", new { user_ID, isNew }).ToList();
        }

        #endregion

        #region Scan

        public int COUNTScan(int user_ID)
        {
            return connection.Query<int>("SELECT COUNT(*) FROM netgarson.dbo.[Scan] WHERE User_ID = @user_ID", new { user_ID }).FirstOrDefault();
        }

        public int COUNTScan_WHEREIsNew(int user_ID, bool isNew)
        {
            return connection.Query<int>("SELECT COUNT(*) FROM netgarson.dbo.[Scan] WHERE User_ID = @user_ID AND IsNew = @isNew", new { user_ID, isNew }).FirstOrDefault();
        }

        public List<Scan> SELECTScan(int user_ID)
        {
            return connection.Query<Scan>("SELECT * FROM netgarson.dbo.[Scan] WHERE User_ID = @user_ID", new { user_ID }).ToList();
        }

        public Scan SELECTScan_WHEREID(int ID)//
        {
            return connection.Query<Scan>("SELECT * FROM netgarson.dbo.[Scan] WHERE ID = @ID", new { ID }).FirstOrDefault();
        }

        public void UPDATEScan_WHEREID(Scan scan)
        {
            connection.Execute("UPDATE netgarson.dbo.[Scan] SET TableNo = @TableNo, IsNew = @IsNew, User_ID = @User_ID WHERE ID = @ID", new { scan.TableNo, scan.IsNew, scan.User_ID, scan.ID });
        }

        public List<Scan> SELECTScan_WHEREIsNew_ORDERBYRecordDate(int user_ID, bool isNew)
        {
            return connection.Query<Scan>("SELECT * FROM netgarson.dbo.[Scan] WHERE User_ID = @user_ID AND IsNew = @isNew ORDER BY RecordDate DESC", new { user_ID, isNew }).ToList();
        }

        #endregion

        #region Set

        public Set SELECTSet(int user_ID)//
        {
            return connection.Query<Set>("SELECT * FROM netgarson.dbo.[Set] WHERE User_ID = @user_ID", new { user_ID }).FirstOrDefault();
        }

        #endregion

        #region Year

        public List<Year> SELECTYear_ORDERBYValue()
        {
            return connection.Query<Year>("SELECT * FROM netgarson.dbo.[Year] ORDER BY Value DESC").ToList();
        }

        public void INSERTYear(Year year)
        {
            connection.Execute("INSERT INTO netgarson.dbo.[Year]([Value]) VALUES (@Value)", new { year.Value });
        }

        #endregion

        #region Year

        public List<YearDecade> SELECTYearDecade_ORDERBYBeginValue()
        {
            return connection.Query<YearDecade>("SELECT * FROM netgarson.dbo.[YearDecade] ORDER BY BeginValue DESC").ToList();
        }

        public void INSERTYearDecade(YearDecade yearDecade)
        {
            connection.Execute("INSERT INTO netgarson.dbo.[YearDecade]([BeginValue], [EndValue]) VALUES (@BeginValue, @EndValue)", new { yearDecade.BeginValue, yearDecade.EndValue });
        }

        #endregion

        public void Close()
        {
            connection.Close();
        }

    }
}