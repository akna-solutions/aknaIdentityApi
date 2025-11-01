using aknaIdentityApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aknaIdentityApi.Infrastructure.Configurations
{
    public class UserTokenEntityConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.DeviceId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.AccessToken)
                .IsRequired()
                .HasMaxLength(2000); // JWT token uzun olabilir

            builder.Property(x => x.RefreshToken)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.AccessTokenExpires)
                .IsRequired();

            builder.Property(x => x.RefreshTokenExpires)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.LastUsedAt)
                .IsRequired();

            builder.Property(x => x.IpAddress)
                .HasMaxLength(50);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);

            builder.Property(x => x.TokenType)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Login");

            builder.Property(x => x.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.RevokedAt)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_UserTokens_UserId");

            builder.HasIndex(x => x.DeviceId)
                .HasDatabaseName("IX_UserTokens_DeviceId");

            builder.HasIndex(x => new { x.UserId, x.DeviceId })
                .HasDatabaseName("IX_UserTokens_UserId_DeviceId");

            builder.HasIndex(x => x.AccessToken)
                .HasDatabaseName("IX_UserTokens_AccessToken");

            builder.HasIndex(x => x.RefreshToken)
                .HasDatabaseName("IX_UserTokens_RefreshToken");

            builder.HasIndex(x => x.AccessTokenExpires)
                .HasDatabaseName("IX_UserTokens_AccessTokenExpires");

            builder.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_UserTokens_IsActive");

            // Foreign key relationship
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}