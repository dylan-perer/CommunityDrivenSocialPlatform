USE [CommunityDrivenSocialPlatform]
GO

INSERT INTO [dbo].[role]([role_name]) VALUES ('USER'), ('ADMIN');

INSERT INTO [dbo].[sub_thread_role]([sub_thread_role_name]) VALUES ('USER'), ('MODERATOR');

INSERT INTO [vote_type] VALUES ('UPVOTE'), ('DOWNVOTE');


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


		   
INSERT INTO [dbo].[sub_thread]
    ([Name]
	,[description]
	,[welcome_message]
    ,[created_at]
    ,[Creator])
	VALUES
		('sub1', 'desc', 'welcomemsg', '2022-03-30', '1'),
		('sub2', 'desc', 'welcomemsg', '2022-03-30', '1'),
		('sub3', 'desc', 'welcomemsg', '2022-03-30', '2');

INSERT INTO [Post]
	([title]
	,[body]
	,[author]
	,[sub_thread_id]
	,[created_at])
	VALUES
		('post1', 'body', '1', '1', '2022-03-30'),
		('post2', 'body', '1', '1','2022-03-30'),
		('post3', 'body', '2', '1','2022-03-30'),
		('post1', 'body', '1', '2', '2022-03-30'),
		('post2', 'body', '1', '2','2022-03-30'),
		('post3', 'body', '2', '3','2022-03-30');

INSERT INTO [vote]
	([user_id],
	[post_id],
	[vote_type_id])
	VALUES
		('1', '2', '1'),
		('1', '1', '1'),
		('2', '1', '1'),
		('3', '2', '1');

INSERT INTO [comment]
	([user_id],
	[post_id],
	[body],
	[created_at])
	VALUES
		('1', '2', 'commentbody', '2022-03-03'),
		('1', '1', 'commentbody', '2022-03-03'),
		('2', '1', 'commentbody', '2022-03-03'),
		('3', '2', 'commentbody', '2022-03-03');
		*/