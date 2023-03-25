


using Microsoft.EntityFrameworkCore;
using Project_Movie_Group6.Models;

namespace Project_Movie_Group6.DAO



{
    public class PersonDAO
    {
        CenimaDBContext db = null;

        public PersonDAO()
        {
            db = new CenimaDBContext();
           
        }

        public PersonDAO(CenimaDBContext context)
        {
            db = context;

        }






        public long Insert(Person entity)
        {
            db.Persons.Add(entity);
            db.SaveChanges();
            return entity.PersonId;
        }
        //public long Update(Person entity) { }
        public void Delete(Person entity) { }

        public Person GetByEmail(String email)
        {
            return db.Persons?.SingleOrDefault(x=>x.Email == email);
        }

        public bool Login(string email, string password)
        {
            var result = db.Persons.Count(x => x.Email == email && x.Password == password);
            //var result = db.Persons.SingleOrDefault(a => a.Email!.Equals(email) && a.Password!.Equals(password));
            if (result > 0)
            {
                return true;
            }
            else { return false; }

        }
    }
}
