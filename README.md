# TravelAgencyApp

This is an application designed for the use of tourism travel agencies selling holiday packages. Travel agencies working alone or large tourism travel agencies which have sub- agencies affiliated to them, will also be able to use the project.

## Technology

* Net Core 5.0.101
* EntityFrameworkCore 5.0.11

## Usage

You can download the application directly to your computer from the download section.
The application has been developped with Code First Data-Access method. Connection string: "server=.;database=TravelAgencyDb; trusted_connection = true;MultipleActiveResultSets=true;".
When the application is launched, the database is created by writing add-migration and then update-database commands to the package manager console. You need to choose Repository as default project in consol screen.

##
The usage of the application will be as follows:

Main structures in the application are definitions, sellers, prices and packages.
There are 8 different definition types used in the application. These are accommodations types, room locations, room types, cars, planes, regions, routes and tours. These defitions are assigned to sellers. For example, hotel is a seller and will be having accommodations types, room locations, room types and region as property.
First, all definitions must be created.

Then sellers will need to be created. There are 6 different sellers which are hotel, rent a car company, transportation company, tour operator, flight company and tour guide.
Once sellers are created then prices must be defined for each seller as both purchase and seller prices as Euro currency in the application.

When this is done, we are ready to create holiday packages. A holiday package includes hotel, tours, flights, transportations from airport to hotel. Optionally it can have a rental car and tour guide. Optional ones will be able to added to package by sales agent while booking a package with customer.

Packages can be listed / filtered in sales agent menu. Sales agent see the package with sales price as both Euro and Turkish Lira calculated according to current Euro/TL exchange rate taken from Central Bank of the Republic of Turkey. Sales agent can add rental car or a tour guide to package if customer wishes and book the sales.

When a sales occurred a sales code is created and sales is saved. All sales and their details can be listed / filtereds on sales menu.

Also when a sales occurred, the sales amount as Turkish Lira calculated by current exchange rate is recorded in the debit section of the current account of the sales code and the purchase prices as Turkish Lira calculated by current exchange rate are recorded in the credit section of the current account of each seller included in the sales.
Current account balance reports can be found per sales or account code on Accounting menu.

Application has 2 different logging. One is user logs which keeps record of which user visited which page and when. The other one is called change history which keeps record of which user changed which property of which entity and when. These logs can be found under Logs menu and accessed only by users with admin role.

#### Roles
* Admin – creates user and roles, has all authorizations. Only role that has access to logging menu.
* Accounting – can access to accounting menu where sales and current accounts reports exist.
* SalesAgent – can access to SalesAgent menu where sales took place. 
* Sales – can access to sales history and details.
* User – can access to Definitions/Seller/Package menus and perform CRUD operations.

## 
At least admin role and a user must be defined in order to use the application. After running the app, please add /Dashboard/Role to url and go to this page. https://localhost:xxxxx/Dashboard/Role and create Admin role. Then create a admin user from Authorization menu / User List by clicking Add User button.

![image](https://user-images.githubusercontent.com/71972947/151670087-9ef2fb11-ef9c-4643-a02b-e96ddefe7f7e.png)

![image](https://user-images.githubusercontent.com/71972947/151670209-db2ed2d3-3eca-4dd9-b71a-868a6e27ed83.png)

Now you can cancel comment lines for [TypeFilter(typeof(AuthFilter))] [Authorize(Roles = "Admin")] in Role and User controllers.

In this project, a user can have 1 role. Same e-mail address and username can be used only once. Roles defined in Role enum under Domain/Enums. If you wish, create the other roles and create a user for them as well.

![image](https://user-images.githubusercontent.com/71972947/151670422-0137ffdb-12f7-41de-811a-5893f97920f7.png)

Now you can log-out by clicking the log-out button on upper right corner and try log-in with created admin role.

When you login with your credentials from home page, you will be redirected to Dashboard.
![image](https://user-images.githubusercontent.com/71972947/151670532-9bdf96cd-fdca-4595-93fd-aeb97586ce1e.png)

Session idle time out is 10 minutes. After that time you will be asked to login with your credentials and if successful you will be directed to requested page.

#### Definitions

![image](https://user-images.githubusercontent.com/71972947/151671734-f1483a05-f3eb-4b1d-90e8-3a1a2bb623c6.png)

Lets create room types. Definitions/Room Types and click on plus icon on upper right corner.
![image](https://user-images.githubusercontent.com/71972947/151671954-892f3d8e-b5b4-4d4c-80cd-2d83f59bd53a.png)

And create below room types. Room type codes must be the same as below, SGL, DBL and TRP.
![image](https://user-images.githubusercontent.com/71972947/151672091-7a631d14-142d-4722-b890-14a5096b0f0c.png)

For other definition types, lets create below ones.
Accomodation types:
![image](https://user-images.githubusercontent.com/71972947/151672337-71da1b86-e768-4037-b669-337036b889bf.png)

Car:
![image](https://user-images.githubusercontent.com/71972947/151672247-6026566f-e922-4652-8511-e1be751afa96.png)

Plane:
![image](https://user-images.githubusercontent.com/71972947/151672295-9300f6d2-c056-4f81-b574-659aa962f51f.png)

Regions will be assigned to sellers to define the area they serve.
![image](https://user-images.githubusercontent.com/71972947/151672376-5cf20fb2-bdf2-4b71-b530-276ff3dafdac.png)

Room Location:
![image](https://user-images.githubusercontent.com/71972947/151672506-a59eb27c-af30-4ce2-b193-a5070669dc08.png)

Routes will be assigned to flight companies and transportation companies.
![image](https://user-images.githubusercontent.com/71972947/151672637-066c2a83-1939-4d7b-ba1a-3e162a204381.png)

Tours will be added to holiday packages. Before creating tours we need to create tour operators which is a seller. Each tour will be assigned to a tour operator.

#### Sellers

Lets create a hotel. Sellers/Hotels and click on plus icon on upper right corner.
While creating a seller, you can choose multiple options from selections. You will be able to add more or remove them once the seller is created from detail section.
For hotel, profile photo is needed. Then you will be able to add more photo from gallery menu.
Sellers codes are given manually, they are also used as current account codes for accounting purposes.
![image](https://user-images.githubusercontent.com/71972947/151675288-4bb301e5-e112-4ac5-99d7-10b8f2a37a44.png)

Once it is created please click detail button.
![image](https://user-images.githubusercontent.com/71972947/151675434-50ddebc5-b0ad-437f-bb8f-bdd4b59fd8da.png)

You can see the all details and make changes if you wish. Prices tab will be empty for now. You can click the gallery button in the upper right corner.
![image](https://user-images.githubusercontent.com/71972947/151675477-8d54f2a6-14c3-41c7-bc1f-7451796ecd47.png)

You can add photos to gallery. These photos will be shown on sales agent screen in package details.
![image](https://user-images.githubusercontent.com/71972947/151675638-e7d754ea-67cc-4418-ad23-f3a5caf62d85.png)

Lets create at least 1 company for each seller.
Flight Company:
![image](https://user-images.githubusercontent.com/71972947/151675741-61eabd63-bb0a-4802-bfd7-65bc4de8224a.png)

Rent A Car Company:
![image](https://user-images.githubusercontent.com/71972947/151675863-5f1a0c14-388d-478a-977f-bfda40344ea6.png)

Transportation Company:
![image](https://user-images.githubusercontent.com/71972947/151675896-e554627b-a3b3-4497-8ebd-894ec8c31138.png)

Tour Guide:
![image](https://user-images.githubusercontent.com/71972947/151675944-8fd0ad16-7e33-4242-8f79-1c33a918e845.png)

Tour Operator:
![image](https://user-images.githubusercontent.com/71972947/151676056-3ef5b511-18fb-4a72-9d32-82ba0dc7463d.png)

Now we can also create tours. Lets create boat and city tours.
![image](https://user-images.githubusercontent.com/71972947/151676136-0027601a-ddd8-4fe6-b93a-a1cfa0aa4851.png)
![image](https://user-images.githubusercontent.com/71972947/151676176-ddf96740-29dd-4cec-a618-2e45d5300406.png)

#### Prices

It is time to define purchase and sales prices for each seller for their services.

Each seller has a price tab on the detail page. On this tab, please click plus icon.
![image](https://user-images.githubusercontent.com/71972947/151676317-4071958c-2ead-4e65-bf07-4e4b0249ba94.png)

Price creation for double room type, full board, general room on June. Only 1 price can be defined for the same combination. This is room price per night.
![image](https://user-images.githubusercontent.com/71972947/151678173-ddfa1a5f-76e9-422c-8e5f-1adfbe7853eb.png)

Price can be seen on price tab now.
![image](https://user-images.githubusercontent.com/71972947/151676600-9885571e-64ed-47d1-ba6e-812cbacf3881.png)

For each seller at least 1 price must be defined for year 2022 and period of June. For flight and tranportation companies, price must be defined in both ways. 
![image](https://user-images.githubusercontent.com/71972947/151676729-34a38ae1-49d0-4621-9a63-deb4c35c30ab.png)

#### Package

Click on plus icon.
![image](https://user-images.githubusercontent.com/71972947/151678018-c2a2718f-0f8b-4d81-a26f-f99793d89f0c.png)

Quota represents available room quantity. Once a sales is occured for the package, quota will be decreased.
![image](https://user-images.githubusercontent.com/71972947/151678229-ea57d674-4acc-411b-8f4c-dd46c9d02ed5.png)

Now it is created. Lets go to package details and add sellers. It's price is zero because we did not define the sellers yet. In each tab click on plus icon to add seller.
![image](https://user-images.githubusercontent.com/71972947/151678313-c0b446bd-8465-424c-a2dc-dd0105cc9adb.png)
When the add button is pressed, only the prices that match on the basis of region, period and year are listed.

Click on choose button.
![image](https://user-images.githubusercontent.com/71972947/151678459-87527c89-a67d-4506-b40f-b9ceb0388cb1.png)

Now hotel is added to package.
![image](https://user-images.githubusercontent.com/71972947/151678480-89aae5c0-f77b-421c-bb51-44d18b092a8b.png)

Please add flights, transportation and tours also. Remember to add price for flight and transportation for arrival and return.

#### Sales Agent

Holiday package is ready. Sales agent can now monitor this package in Sales Agent menu and make the sale to customers who visit the agency.
Packages can be filtered according to period and region.
![image](https://user-images.githubusercontent.com/71972947/151678623-0f9fdcd7-f83b-4f55-8841-c5b2dc7ee208.png)

Sales Agent can add rental car or tour guide to the package and monitor the package sales price in Turkish Lira calculated by currnet exchange rate.
![image](https://user-images.githubusercontent.com/71972947/151678646-f84d05f4-c9f4-4663-b9f0-34f4c6353c56.png)

Lets click on Add Rental Car Button

Available prices are listed. Dates must be choosen. Then click on the choose botton on the right.
![image](https://user-images.githubusercontent.com/71972947/151678705-618a91b0-f2b8-4401-bd91-531135b7d352.png)

Now rental car added to package and price is re-calculated. Click on the purchase button on upper left top.
![image](https://user-images.githubusercontent.com/71972947/151678777-d62cc101-9001-45bc-a8da-bcbfad6d09ce.png)

We are required to enter contact information for the sale. Provide the info and click on pruchase button.
![image](https://user-images.githubusercontent.com/71972947/151678850-404588d9-bf8c-49b5-acb3-f646c28f5cab.png)

Sales is done. Package quota is decreased to 1 from 2.

#### Sales Reports

Under sales menu, all sales are listed and can be filtered by region or date.
![image](https://user-images.githubusercontent.com/71972947/151678933-64e3eea0-2e1d-40a2-8252-99702b714a2c.png)

In the details, total sales price and prices separated by each seller can be found.
![image](https://user-images.githubusercontent.com/71972947/151678970-d574b420-edf4-4b61-849b-3b624122b2cd.png)

#### Accounting Reports

Once a sales is occurred, debits and credits of current accounts are recorded. 
The sales amount as Turkish Lira calculated by current exchange rate is recorded in the debit section of the current account of the sales code and the purchase prices as Turkish Lira calculated by current exchange rate are recorded in the credit section of the current account of each seller included in the sales.

Current account balance reports can be found per sales or account code on Accounting menu.
![image](https://user-images.githubusercontent.com/71972947/151679042-be899e0d-e6c8-41b0-b672-ccfd988e20be.png)

For sales with code S5000 details are below. Revenue is 5.350,4 TL.
![image](https://user-images.githubusercontent.com/71972947/151679121-ecfa59a8-24f7-45af-84ba-418a0f5c5b5e.png)

For current account balance reports, dates and code is entered then report is generated.
![image](https://user-images.githubusercontent.com/71972947/151679171-ab5c8590-9756-4e61-bcd8-7e5a54d1e658.png)

For the seller with code O001 and dates between 1.01.2022 and 30.01.2022 current account balance is like below:
![image](https://user-images.githubusercontent.com/71972947/151679202-cbe44780-b72d-4b36-89bb-85dfeba92c8a.png)

#### Logs

User logs keeps record of which user visited which page and when. 
![image](https://user-images.githubusercontent.com/71972947/151679367-a4721e57-a4f3-45f0-8d2f-76d03daee0c2.png)

Change history keeps record of which user changed which property of which entity and when.
![image](https://user-images.githubusercontent.com/71972947/151679456-985a5d65-dcdb-418f-868d-c3ca82a3e04d.png)

## Contact

You can reach me via engin.tayfun@gmail.com.

Thank you.
