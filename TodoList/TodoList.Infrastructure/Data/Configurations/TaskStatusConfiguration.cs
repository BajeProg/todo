using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskStatusEntity = TodoList.Domain.Entities.TaskStatus;

namespace TodoList.Infrastructure.Data.Configurations
{
    public class TaskStatusConfiguration
        : IEntityTypeConfiguration<TaskStatusEntity>
    {
        public void Configure(EntityTypeBuilder<TaskStatusEntity> builder)
        {
            builder.ToTable(
                "task_statuses",
                tableBuilder => tableBuilder.HasCheckConstraint(
                    "ck_task_statuses_color_hex",
                    "color ~ '^#[0-9A-Fa-f]{6}$'"));

            builder.HasKey(x => x.Id)
                .HasName("pk_task_statuses");

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.NormalizedName)
                .HasColumnName("normalized_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Color)
                .HasColumnName("color")
                .HasMaxLength(7)
                .IsRequired();

            builder.Property(x => x.IsSystem)
                .HasColumnName("is_system")
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasIndex(x => x.NormalizedName)
                .IsUnique()
                .HasDatabaseName("ux_task_statuses_normalized_name");

            builder.HasData(new
            {
                Id = TaskStatusEntity.OpenId,
                Name = TaskStatusEntity.OpenName,
                NormalizedName = "ОТКРЫТА",
                Color = TaskStatusEntity.OpenColor,
                IsSystem = true
            });
        }
    }
}
