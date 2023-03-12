using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoriesWeb.Models;

namespace StoriesWeb.Data
{
  public class ApplicationDbContext : IdentityDbContext<UserModel>
  {
    public DbSet<ActiveConnection> ActiveConnections { get; set; }
    public DbSet<Administrator> Administrators { get; set; }
    public DbSet<AgeRestriction> AgeRestrictions { get; set; }
    public DbSet<ApplicationSetup> ApplicationSetups { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Article_Category> ArticleCategories { get; set; }
    public DbSet<Article_Collection> ArticleCollections { get; set; }
    public DbSet<Article_Read> ArticleReads { get; set; }
    public DbSet<ArticleTranslation> Translations { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryGroup> CategoryGroups { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Club_Article> ClubArticles { get; set; }
    public DbSet<Club_User> ClubUsers { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Collection_Categories> CollectionCategories { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Critic> Critics { get; set; }
    public DbSet<EmailLog> EmailLogs { get; set; }
    public DbSet<EmailRecepient> EmailRecepients { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<ChatBans> ChatBans { get; set; }
    public DbSet<ChatElevated> ChatsElevated { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Chatroom> Chatrooms { get; set; }
    public DbSet<ChatroomAdmin> ChatroomAdmins { get; set; }
    public DbSet<ChattingTime> ChattingTimes { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageRecepient> MessageRecepients { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SpamReport> SpamReports { get; set; }
    public DbSet<Star> Stars { get; set; }
    public DbSet<User_Favorite> UserFavorites { get; set; }
    public DbSet<UsersInRoom> UsersInRoom { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<ActiveConnection>().ToTable("ActiveConnections")
          .HasOne(s => s.User).WithMany(s => s.ActiveConnections)
          .HasForeignKey(s => s.UserId);

      builder.Entity<Administrator>().ToTable("Administrators")
          .HasOne(s => s.User).WithMany(s => s.Admins)
          .HasForeignKey(s => s.UserId);
      builder.Entity<Administrator>()
          .HasOne(s => s.Activator)
          .WithMany(s => s.Activators)
          .HasForeignKey(s => s.ActivatedById);
      builder.Entity<Administrator>()
          .HasOne(s => s.Deactivator)
          .WithMany(s => s.Deaktivators)
          .HasForeignKey(s => s.DeactivatedById);

      builder.Entity<AgeRestriction>().ToTable("AgeRestrictions");

      builder.Entity<ApplicationSetup>().ToTable("ApplicationSetup")
        .HasOne(s => s.Owner)
        .WithOne(s => s.ApplicationSetup)
        .HasForeignKey<ApplicationSetup>(s => s.OwnerId);

      builder.Entity<Article>().ToTable("Articles")
          .HasOne(s => s.AgeRestriction)
          .WithMany(s => s.Articles)
          .HasForeignKey(s => s.AgeRestrictionId);
      builder.Entity<Article>()
          .HasOne(s => s.Author)
          .WithMany(s => s.Articles)
          .HasForeignKey(s => s.AuthorId);
      builder.Entity<Article>()
          .HasOne(s => s.Banner)
          .WithMany(s => s.ArticlesBans)
          .HasForeignKey(s => s.BannedById);
      builder.Entity<Article>()
        .HasOne(s => s.Language)
        .WithMany(s => s.Articles)
        .HasForeignKey(s => s.SourceLanguageId);

      builder.Entity<Article_Category>().ToTable("ArticleCategories")
          .HasOne(s => s.Article)
          .WithMany(s => s.ArticleCategories)
          .HasForeignKey(s => s.ArticleId);
      builder.Entity<Article_Category>()
          .HasOne(s => s.Category)
          .WithMany(s => s.ArticleCategories)
          .HasForeignKey(s => s.CategoryId);

      builder.Entity<Article_Collection>().ToTable("ArticleCollections")
          .HasOne(s => s.Article)
          .WithMany(s => s.Article_Collections)
          .HasForeignKey(s => s.ArticleId);
      builder.Entity<Article_Collection>()
          .HasOne(s => s.Collection)
          .WithMany(s => s.Article_Collections)
          .HasForeignKey(s => s.CollectionId);

      builder.Entity<Article_Read>().ToTable("ArticleReads")
          .HasOne(s => s.Article)
          .WithMany(s => s.Article_Reads)
          .HasForeignKey(s => s.ArticleId);
      builder.Entity<Article_Read>()
          .HasOne(s => s.User)
          .WithMany(s => s.Article_Reads)
          .HasForeignKey(s => s.UserId);

      builder.Entity<ArticleTranslation>().ToTable("Translations")
        .HasOne(s => s.Article)
        .WithMany(s => s.Translations)
        .HasForeignKey(s => s.ArticleId);
      builder.Entity<ArticleTranslation>()
        .HasOne(s => s.Language)
        .WithMany(s => s.Translations)
        .HasForeignKey(s => s.LanguageId);

      builder.Entity<Category>().ToTable("Categories")
          .HasOne(s => s.CategoryGroup)
          .WithMany(s => s.Categories)
          .HasForeignKey(s => s.CategoryGroupId);

      builder.Entity<CategoryGroup>().ToTable("CategoryGroups");

      builder.Entity<Club>().ToTable("Clubs")
          .HasOne(s => s.User)
          .WithMany(s => s.Clubs)
          .HasForeignKey(s => s.OwnerId);

      builder.Entity<Club_Article>().ToTable("ClubArticles")
          .HasOne(s => s.Article)
          .WithMany(s => s.Club_Articles)
          .HasForeignKey(s => s.ArticleId);
      builder.Entity<Club_Article>()
          .HasOne(s => s.Club)
          .WithMany(s => s.Club_Articles)
          .HasForeignKey(s => s.ClubId);

      builder.Entity<Club_User>().ToTable("ClubUsers")
          .HasOne(s => s.User)
          .WithMany(s => s.Club_Users)
          .HasForeignKey(s => s.UserId);
      builder.Entity<Club_User>()
          .HasOne(s => s.Club)
          .WithMany(s => s.Club_Users)
          .HasForeignKey(s => s.ClubId);

      builder.Entity<Collection>().ToTable("Collections")
          .HasOne(s => s.User)
          .WithMany(s => s.Collections)
          .HasForeignKey(s => s.UserId);

      builder.Entity<Collection_Categories>().ToTable("CollectionCategories")
          .HasOne(s => s.Collection)
          .WithMany(s => s.Collection_Categories)
          .HasForeignKey(s => s.CollectionId);
      builder.Entity<Collection_Categories>()
          .HasOne(s => s.Category)
          .WithMany(s => s.CollectionCategories)
          .HasForeignKey(s => s.CategoryId);

      builder.Entity<Country>().ToTable("Countries");

      builder.Entity<Critic>().ToTable("Critics")
          .HasOne(s => s.Article)
          .WithMany(s => s.Critics)
          .HasForeignKey(s => s.ArticleId);
      builder.Entity<Critic>()
          .HasOne(s => s.User)
          .WithMany(s => s.Critics)
          .HasForeignKey(s => s.UserId);

      builder.Entity<EmailLog>().ToTable("EmailLogs");

      builder.Entity<EmailRecepient>().ToTable("EmailRecepients")
          .HasOne(s => s.EmailLog)
          .WithMany(s => s.EmailRecepients)
          .HasForeignKey(s => s.EmailLogId);

      builder.Entity<Friend>().ToTable("Friends")
          .HasOne(s => s.User)
          .WithMany(s => s.Friends)
          .HasForeignKey(s => s.UserId);
      builder.Entity<Friend>()
          .HasOne(s => s.UserFriend)
          .WithMany(s => s.Befriended)
          .HasForeignKey(s => s.FriendId);

      builder.Entity<ChatBans>().ToTable("ChatBans")
          .HasOne(s => s.User)
          .WithMany(s => s.ChatBansUsers)
          .HasForeignKey(s => s.UserId);
      builder.Entity<ChatBans>()
          .HasOne(s => s.AdminUser)
          .WithMany(s => s.ChatBansAdmins)
          .HasForeignKey(s => s.AdminUserId);
      builder.Entity<ChatBans>()
          .HasOne(s => s.Chatroom)
          .WithMany(s => s.ChatBans)
          .HasForeignKey(s => s.ChatroomId);

      builder.Entity<ChatElevated>().ToTable("ChatsElevated")
          .HasOne(s => s.User)
          .WithMany(s => s.ChatElevatedUsers)
          .HasForeignKey(s => s.UserId);

      builder.Entity<ChatMessage>().ToTable("ChatMessages")
          .HasOne(s => s.Recepient)
          .WithMany(s => s.ChatMessageRecepient)
          .HasForeignKey(s => s.RecepientId);
      builder.Entity<ChatMessage>()
          .HasOne(s => s.Sender)
          .WithMany(s => s.ChatMessageSenders)
          .HasForeignKey(s => s.SenderId);
      builder.Entity<ChatMessage>()
          .HasOne(s => s.Chatroom)
          .WithMany(s => s.ChatMessages)
          .HasForeignKey(s => s.ChatroomId);

      builder.Entity<Chatroom>().ToTable("Chatrooms")
          .HasOne(s => s.Owner)
          .WithMany(s => s.Chatrooms)
          .HasForeignKey(s => s.OwnerId);

      builder.Entity<ChatroomAdmin>().ToTable("ChatroomAdmins")
          .HasOne(s => s.User)
          .WithMany(s => s.ChatroomAdmins)
          .HasForeignKey(s => s.UserId);
      builder.Entity<ChatroomAdmin>()
          .HasOne(s => s.Chatroom)
          .WithMany(s => s.ChatroomAdmins)
          .HasForeignKey(s => s.RoomId);

      builder.Entity<ChattingTime>().ToTable("ChattingTimes")
          .HasOne(s => s.User)
          .WithMany(s => s.ChattingTimes)
          .HasForeignKey(s => s.UserId);

      builder.Entity<Language>().ToTable("Languages");

      builder.Entity<Like>().ToTable("Likes")
          .HasOne(s => s.User)
          .WithMany(s => s.Likes)
          .HasForeignKey(s => s.UserId);
      builder.Entity<Like>()
          .HasOne(s => s.Article)
          .WithMany(s => s.Likes)
          .HasForeignKey(s => s.ArticleId);

      builder.Entity<Message>().ToTable("Messages")
          .HasOne(s => s.User)
          .WithMany(s => s.Messages)
          .HasForeignKey(s => s.SenderId);

      builder.Entity<MessageRecepient>().ToTable("MessageRecepients")
          .HasOne(s => s.Message)
          .WithMany(s => s.Recepients)
          .HasForeignKey(s => s.MessageId);
      builder.Entity<MessageRecepient>()
          .HasOne(s => s.Recepient)
          .WithMany(s => s.MessageRecepients)
          .HasForeignKey(s => s.RecepientId);

      builder.Entity<Session>().ToTable("Sessions")
          .HasOne(s => s.User)
          .WithMany(s => s.Sessions)
          .HasForeignKey(s => s.UserId);

      builder.Entity<SpamReport>().ToTable("SpamReports")
          .HasOne(s => s.Message)
          .WithMany(s => s.SpamReports)
          .HasForeignKey(s => s.MessageId);
      builder.Entity<SpamReport>()
          .HasOne(s => s.User)
          .WithMany(s => s.SpamReportingUsers)
          .HasForeignKey(s => s.ReportedById);
      builder.Entity<SpamReport>()
          .HasOne(s => s.Admin)
          .WithMany(s => s.SpamResolvingAdmins)
          .HasForeignKey(s => s.AdminUserId);

      builder.Entity<Star>().ToTable("Stars")
          .HasOne(s => s.Article)
          .WithMany(s => s.Stars)
          .HasForeignKey(s => s.ArticleId);
      builder.Entity<Star>()
          .HasOne(s => s.User)
          .WithMany(s => s.Stars)
          .HasForeignKey(s => s.UserId);

      builder.Entity<User_Favorite>().ToTable("UserFavorites")
          .HasOne(s => s.User)
          .WithMany(s => s.user_Favorites)
          .HasForeignKey(s => s.UserId);
      builder.Entity<User_Favorite>()
          .HasOne(s => s.Author)
          .WithMany(s => s.user_FavoriteUsers)
          .HasForeignKey(s => s.AuthorId);

      builder.Entity<UserModel>()
          .HasOne(s => s.Country)
          .WithMany(s => s.Users)
          .HasForeignKey(s => s.CountryId);

      builder.Entity<UsersInRoom>().ToTable("UsersInRoom")
          .HasOne(s => s.User)
          .WithMany(s => s.UsersInRoom)
          .HasForeignKey(s => s.UserId);
      builder.Entity<UsersInRoom>()
          .HasOne(s => s.Chatroom)
          .WithMany(s => s.UsersInRoom)
          .HasForeignKey(s => s.RoomId);
    }
  }
}