using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            } else
            {
                return user.ID;
            }
            
        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
           
        }
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            var data = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                return data.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString)).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            return data.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public User GetById(string username)
        {
            return db.Users.SingleOrDefault(x => x.UserName == username);
        }

        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }
        public int Login(string userName, string password)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if(result == null)
            {
                return 0;
            } else
            {
                if (result.Status == false)
                {
                    return -1;
                } else
                {
                    if(result.Password == password)
                    {
                        return 1;
                    } else
                    {
                        return -2;
                    }
                }
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            } catch(Exception)
            {
                return false;
            }
            
        }
        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        public bool CheckUserName (string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;
        }

        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }

    }
}
