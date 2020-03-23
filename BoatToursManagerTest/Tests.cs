using BoatToursManager;
using BoatToursManager.BL;
using BoatToursManager.Controllers;
using BoatToursManager.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BoatToursManagerTest
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestUserRegistration()
        {
            //Arrange
            UserController uc = new UserController();
            var userRegister = new UserRegistrationModel
            {
                name = "ram",
                email = "ram@gmail.com",
                password = "ram123456",
                phone = "1234423434",
                addressName = "kiel",
                streetName = "westring 282",
                location = "kiel, Germany",
                postalCode = "12344",
                country = "Germany"
            };
            var address = new Address(userRegister.addressName, userRegister.streetName, userRegister.location, userRegister.postalCode, userRegister.country);
            User newUser = new User(userRegister.name, userRegister.email,
                    userRegister.password, userRegister.phone, UserType.CLIENT, address);
            //Act
            address.saveInDB();
            bool result = MainClass.Instance.addUser(newUser);
            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestEmailExistOrNot()
        {
            //Arrange
            var email = "karkikiran714@gmail.com";
            //Act
            bool result = MainClass.Instance.getUsers().Find(v => v.email == email) != null;
            //Assert
            Assert.IsTrue(result);
        }



        [TestMethod]
        public void TestUserLogin()
        {
            //Arrange
            var result = false;
            var userModel = new UserLoginModel
            {
                email = "karkikiran714@gmail.com",
                password = "123456"
            };
            if (userModel.email != null && userModel.password != null)
            {
                //Act
                User userExist = MainClass.Instance.getUsers().Find(v => v.email == userModel.email);
                if (userExist != null)
                {
                    if (userExist.isEmailVerified)
                    {
                        if (string.Compare(CryptoPass.Hash(userModel.password), userExist.password) == 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            //Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void TestAddBoat()
        {
            bool result = false;
            BoatModel boatModel = new BoatModel { name = "boat 1", capacity = 300, pricePerHour = 30, seasonId = 1, imagePath = "Public/images" };
            LatLongCoordinate point = new LatLongCoordinate(5, 4, "asd");
            point.saveInDB();
            // Get location
            Location location = MainClass.Instance.getLocations().Find(v => v.id == 3007);
            if (MainClass.Instance.addLocation(new Location(point, location.name)))
            {
                if (location != null)
                {
                    if (location.addBoat(new Boat(boatModel.name, boatModel.capacity, boatModel.pricePerHour, boatModel.seasonId, boatModel.imagePath)))
                    {
                        result = true;
                    }
                }
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestAddSchedulePlan()
        {
            SchedulePlanModel sp = new SchedulePlanModel
            {
                beginDate = DateTime.Now.AddDays(5),
                endDate = DateTime.Now.AddDays(4),
            };
            var result = MainClass.Instance.addSchedulePlan(new SchedulePlan(sp.beginDate, sp.endDate));
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CheckLocationExistOrNot()
        {
            BoatController controller = new BoatController();
            bool exist = controller.CheckLocation(3007);
            Assert.IsTrue(exist);
        }
        [TestMethod]
        public void GetBoatsByLocationId()
        {
            BoatController controller = new BoatController();
            bool exist = controller.CheckLocation(3007);
            Assert.IsTrue(exist);
        }
        [TestMethod]
        public void CheckViewResult()
        {
            string expected = "Boats";
            BoatController controller = new BoatController();

            var result = controller.GetBoats() as ViewResult;

            Assert.AreEqual(expected, result.ViewName);
        }
        [TestMethod]
        public void CommentOnBoat()
        {
            CommentModel model = new CommentModel
            {
                comment = "this boat is perfect",
                commentDate = DateTime.Now,
                boatId = 5010,
                userId = 11
            }; ;
            CommentBL com = new CommentBL();
            var result = com.SaveComment(model);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CheckComment()
        {
            var expected = "Not Empty";
            var comment = "this is comment";
            BoatController com = new BoatController();
            var result = com.checkNull(comment);
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void CheckNullComment()
        {

            BoatController com = new BoatController();
            var result = com.checkNull(null);
            Assert.Equals(result, "Empty");
        }
    }
}
