using CafezinhoELivrosApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafezinhoELivrosApi.Data.Mappings;

public class UserMapping: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder) {
        builder.ToTable("Users");
        //builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
        builder.Property(x => x.CurrentThought).HasMaxLength(30);
        builder.Property(u => u.Description).HasMaxLength(300);
        builder.Property(u => u.City).HasMaxLength(20);
        builder.Property(u => u.State).HasMaxLength(20);

        builder.HasOne(u => u.Role)
               .WithMany()
               .HasForeignKey(u => u.RoleId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }

}
