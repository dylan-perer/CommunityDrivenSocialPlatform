USE [CommunityDrivenSocialPlatform]
GO

INSERT INTO [dbo].[role]([role_name]) VALUES ('USER'), ('ADMIN');

INSERT INTO [dbo].[sub_thread_role]([sub_thread_role_name]) VALUES ('USER'), ('MODERATOR');
/*

INSERT INTO [dbo].[User]
           ([username]
		   ,[email_address]
           ,[password]
           ,[profile_picture_url]
           ,[created_at]
		   ,[role_id]
		   ,[description])
     VALUES
           ('elise', 'elise@gmail.com', 'pswd', null, '2022-03-14', '2', 'desc'), 
		   ('dylan', 'dylan@gmail.com' ,'pswd', null, '2022-03-29', '2', 'desc'),
		   ('jasmine', 'jasmine@gmail.com', 'pswd', null, '2022-03-30', '1', 'desc');


		   
INSERT INTO [dbo].[Subthread]
    ([Name]
    ,[CreatedAt]
    ,[Creator])
	VALUES
		('sub1', '2022-03-30', '1'),
		('sub2', '2022-03-30', '1'),
		('sub3', '2022-03-30', '2');

		*/