using System;
using FMASolutionsCore.CLITools.CLIHelper;
using System.Collections.Generic;
using FMASolutionsCore.BusinessServices.ShoppingService;
using FMASolutionsCore.BusinessServices.BusinessCore.CustomModel;
using FMASolutionsCore.BusinessServices.SQLAppConfigTypes;

namespace FMASolutionsCore.CLITools.ShoppingTester
{
    public class ShoppingTester
    {
        private enum OperationTypes
        {
            Invalid,
            Create,
            Search,
            DisplayAll,
        }

        private enum EntityTypes
        {
            Invalid,
            ProductGroup,
            SubGroup
        }



        public ShoppingTester()
        {
            _firstRun = true;
        }

        private bool _firstRun;
        private EntityTypes _entityType;
        private OperationTypes _operationType;

        public void Run()
        {
            Console.WriteLine("Welcome to Shop Tester!");
            _entityType = GetEntityType();
            _operationType = GetOperationType();
            ProcessRequest(_entityType, _operationType);
        }

        private EntityTypes GetEntityType()
        {
            Console.WriteLine("Please select from the following Entity Types:");
            int i = 0;
            foreach (string enumName in Enum.GetNames(typeof(EntityTypes)))
            {
                if (enumName != EntityTypes.Invalid.ToString())
                    Console.WriteLine(i.ToString() + ") Select \"" + i.ToString() + "\" For: " + enumName);
                i++;
            }

            string returnString = Helper.GetUserInput();

            try
            {
                return (EntityTypes)int.Parse(returnString);
            }
            catch (Exception)
            {
                Helper.DisplayRetryOrQuit();
                return GetEntityType();
            }
        }
        private OperationTypes GetOperationType()
        {
            Console.WriteLine("Please select from the following Entity Types:");
            int i = 0;
            foreach (string enumName in Enum.GetNames(typeof(OperationTypes)))
            {
                if (enumName != EntityTypes.Invalid.ToString())
                    Console.WriteLine(i.ToString() + ") Select \"" + i.ToString() + "\" For: " + enumName);
                i++;
            }

            string returnString = Helper.GetUserInput();

            try
            {
                return (OperationTypes)int.Parse(returnString);
            }
            catch (Exception)
            {
                Helper.DisplayRetryOrQuit();
                return GetOperationType();
            }
        }

        private void ProcessRequest(EntityTypes entityType, OperationTypes operationType)
        {
            if (operationType == OperationTypes.Create)
                GetEntityOptionsForCreate(entityType);
            else if (operationType == OperationTypes.Search)
                GetEntityOptionsForSearch(entityType);
            else if (operationType == OperationTypes.DisplayAll)
                DisplayAll(entityType);
        }

        private Dictionary<string, string> GetEntityOptionsForCreate(EntityTypes entityType)
        {
            Dictionary<string, string> entityOptions = new Dictionary<string, string>();
            if (entityType == EntityTypes.ProductGroup)
            {
                Console.WriteLine("Enter Code for creation:");
                entityOptions.Add("Code", Helper.GetUserInput());
                Console.WriteLine("Enter Name for creation:");
                entityOptions.Add("Name", Helper.GetUserInput());
                Console.WriteLine("Enter Description for creation:");
                entityOptions.Add("Description", Helper.GetUserInput());
                PerformCreate(entityType, entityOptions);
            }
            else if (entityType == EntityTypes.SubGroup)
                throw new NotImplementedException();
            return entityOptions;
        }
        private Dictionary<string, string> GetEntityOptionsForSearch(EntityTypes entityType)
        {
            Dictionary<string, string> entityOptions = new Dictionary<string, string>();
            if (entityType == EntityTypes.ProductGroup)
            {
                Console.WriteLine("Enter ID to search on (1 - 9,999,999):");
                entityOptions.Add("ID", Helper.GetUserInput());
                Console.WriteLine("Enter Code to search on (Max 5 char length):");
                entityOptions.Add("Code", Helper.GetUserInput());
                PerformSearch(entityType, entityOptions);
            }
            else if (entityType == EntityTypes.SubGroup)
                throw new NotImplementedException();
            return entityOptions;
        }
        private void DisplayAll(EntityTypes entityType)
        {
            if (entityType == EntityTypes.ProductGroup)
            {

            }
            else
                throw new NotImplementedException();
        }

        private void PerformSearch(EntityTypes entityType, Dictionary<string, string> options)
        {
            if (entityType == EntityTypes.ProductGroup)
            {   
                string connectionString = Program.appConfig.GetSetting(AppSettings.DBConnectionString.ToString());
                SQLAppConfigTypes dbType = (SQLAppConfigTypes)int.Parse(Program.appConfig.GetSetting(AppSettings.DBType.ToString()));                

                IProductGroupService service = new ProductGroupService(connectionString, dbType);                

                ProductGroup found = service.GetByID(int.Parse(options.GetValueOrDefault("ID")));
                
                if (found != null)
                {
                    Console.WriteLine("Match Found!!!!");
                }
                else
                {
                    Console.WriteLine("No Match Found :(");
                }
            }
            else
                throw new NotImplementedException();

        }
        private void PerformCreate(EntityTypes entityType, Dictionary<string, string> options)
        {
            if (entityType == EntityTypes.ProductGroup)
            {

            }
            else
                throw new NotImplementedException();
        }
    }
}