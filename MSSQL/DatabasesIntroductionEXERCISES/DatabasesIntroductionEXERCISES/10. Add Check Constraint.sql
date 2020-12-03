ALTER TABLE Users
ADD CONSTRAINT CHK_Users_PasswordLength
CHECK(LEN([Password]) >=5)