
CREATE TRIGGER Base_U_UT 
   ON  Material_Base 
   AFTER INSERT,UPDATE
AS 
BEGIN
	update Material_Base set Update_Time=(select GETDATE()) where Id = (select Id from inserted)
END

CREATE TRIGGER Type_U_UT 
   ON  Material_Type
   AFTER INSERT,UPDATE
AS 
BEGIN
	update Material_Type set Update_Time=(select GETDATE()) where Id = (select Id from inserted)
END

CREATE TRIGGER Product_U_UT 
   ON  Material_Product
   AFTER INSERT,UPDATE
AS 
BEGIN
	update Material_Product set Update_Time=(select GETDATE()) where Id = (select Id from inserted)
END

CREATE TRIGGER Company_U_UT 
   ON  Material_Company
   AFTER INSERT,UPDATE
AS 
BEGIN
	update Material_Company set Update_Time=(select GETDATE()) where Id = (select Id from inserted)
END

CREATE TRIGGER Base_Type_U_UT 
   ON  Material_Base_Type
   AFTER INSERT,UPDATE
AS 
BEGIN
	update Material_Base_Type set Update_Time=(select GETDATE()) where Id = (select Id from inserted)
END

CREATE TRIGGER Base_Product_U_UT 
   ON  Material_Base_Product 
   AFTER INSERT,UPDATE
AS 
BEGIN
	update Material_Base_Product set Update_Time=(select GETDATE()) where Id = (select Id from inserted)
END

CREATE TRIGGER Base_Company_U_UT 
   ON  Material_Base_Company
   AFTER INSERT,UPDATE
AS 
BEGIN
	update Material_Base_Company set Update_Time=(select GETDATE()) where Id = (select Id from inserted)
END
