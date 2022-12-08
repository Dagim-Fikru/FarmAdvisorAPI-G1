﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FarmAdvisor.DataAccess.MSSQL.DataContext;
using FarmAdvisor.DataAccess.MSSQL.Entities;
using FarmAdvisor.DataAccess.MSSQL.Functions.Crud;

namespace FarmAdvisor.Function.Test.DataAcess.MSSQLTest
{
    public class CRUDTest
    {
        Mock<DatabaseContext> MockDbContext = new Mock<DatabaseContext>();



        [Fact]
        public async Task Positive_CreateUser()
        {
            var UserMock = new Mock<DbSet<User>>();
            var User = new User
            {
                Name = "Test",
                Email = "Test@test.com",
                AuthId = "admin",
                Phone = "1234567890"
            };

            UserMock.Setup(us => us.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback((User user, CancellationToken token) => { })
                .Returns((User user, CancellationToken token) =>  ValueTask.FromResult((EntityEntry<User>)null) );
            MockDbContext.Setup(us=>us.Set<User>()).Returns(UserMock.Object);
            MockDbContext.Setup(us => us.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));

            var UserCrud = new CRUD();
            var result = await UserCrud.Create(User);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Negative_CreateUser()
        {
            var UserMock = new Mock<DbSet<User>>();
            
            var User = new User
            {};

            UserMock.Setup(us => us.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback((User user, CancellationToken token) => { })
                .Returns((User user, CancellationToken token) => ValueTask.FromResult((EntityEntry<User>)null));
            MockDbContext.Setup(us => us.Set<User>()).Returns(UserMock.Object);
            MockDbContext.Setup(us => us.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));

            var UserCrud = new CRUD();
    

            Assert.ThrowsAsync<DbUpdateException>(async ()=>await UserCrud.Create(User));
        }

        [Fact]
        public async Task Positive_Find()
        {
            var UserMock = new Mock<DbSet<User>>();
            
            UserMock.Setup(us => us.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback((User user, CancellationToken token) => { })
                .Returns((User user, CancellationToken token) => ValueTask.FromResult((EntityEntry<User>)null));
            MockDbContext.Setup(us => us.Set<User>()).Returns(UserMock.Object);
            MockDbContext.Setup(us => us.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));

            var UserCrud = new CRUD();
            var result = await UserCrud.FindAll<User>();

            Assert.NotNull(result);
                
        }


        [Fact]
        public async Task Positive_FindOne()
        {
            var UserMock = new Mock<DbSet<User>>();
            

            UserMock.Setup(us => us.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback((User user, CancellationToken token) => { })
                .Returns((User user, CancellationToken token) => ValueTask.FromResult((EntityEntry<User>)null));
            MockDbContext.Setup(us => us.Set<User>()).Returns(UserMock.Object);
            MockDbContext.Setup(us => us.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));

            var UserCrud = new CRUD();
            var result = await UserCrud.Find<User>(new Guid("A3D55EF6-2454-46B9-3CDB-08DAD942A761"));

            Assert.NotNull(result);
            Assert.Contains("Test", result.Name);
        }

        [Fact]
        public async Task Negative_FindOne()
        {
            var UserMock = new Mock<DbSet<User>>();


            UserMock.Setup(us => us.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback((User user, CancellationToken token) => { })
                .Returns((User user, CancellationToken token) => ValueTask.FromResult((EntityEntry<User>)null));
            MockDbContext.Setup(us => us.Set<User>()).Returns(UserMock.Object);
            MockDbContext.Setup(us => us.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));

            var UserCrud = new CRUD();
            var result = await UserCrud.Find<User>(new Guid("D5367A3A-D98C-4A24-11F4-05DAD77C01C2"));

            Assert.Null(result);
            
        }

        [Fact]
        public async Task Positive_Update()
        {
            var UserMock = new Mock<DbSet<User>>();

            var NewUser = new User
            { 
              UserID = new Guid("D5367A3A-D98B-4A24-11F4-08DAD77C01C2"),
              Name = "updates",
              Phone= "111111111111",
              Email  = "updated@email.com",
              AuthId = "updated"
            };

            UserMock.Setup(us => us.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback((User user, CancellationToken token) => { })
                .Returns((User user, CancellationToken token) => ValueTask.FromResult((EntityEntry<User>)null));
            MockDbContext.Setup(us => us.Set<User>()).Returns(UserMock.Object);
            MockDbContext.Setup(us => us.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));

            var UserCrud = new CRUD();

            var result = await UserCrud.Update<User>(new Guid("D5367A3A-D98B-4A24-11F4-08DAD77C01C2"), NewUser);


            Assert.Contains("updates", result.Name);
            Assert.Contains("1111111111", result.Phone);
            Assert.Contains("updated@email.com", result.Email);
            Assert.Contains("updated", result.AuthId);

            var newResult = await UserCrud.Find<User>(new Guid("D5367A3A-D98B-4A24-11F4-08DAD77C01C2"));

            Assert.Equal(newResult.Name, result.Name);

        }

        [Fact]
        public async Task Positive_Delete()
        {
            var UserMock = new Mock<DbSet<User>>();


            UserMock.Setup(us => us.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback((User user, CancellationToken token) => { })
                .Returns((User user, CancellationToken token) => ValueTask.FromResult((EntityEntry<User>)null));
            MockDbContext.Setup(us => us.Set<User>()).Returns(UserMock.Object);
            MockDbContext.Setup(us => us.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(1));

            var UserCrud = new CRUD();
            
            var allUsers = await UserCrud.FindAll<User>();

            Guid sacId = allUsers[0].UserID;

            var result = await UserCrud.Delete<User>(sacId);
            var AfterDeleteUsers = await UserCrud.FindAll<User>();

            Assert.Equal(AfterDeleteUsers.Count, allUsers.Count - 1);
            Assert.True(result);
          


           
        }




    }
}
