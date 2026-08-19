using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure.Data.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable(
                "task_items",
                tableBuilder => tableBuilder.HasCheckConstraint(
                    "ck_task_items_story_points_non_negative",
                    "story_points IS NULL OR story_points >= 0"));

            builder.HasKey(x => x.Id)
                .HasName("pk_task_items");

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(2000);

            builder.Property(x => x.StoryPoints)
                .HasColumnName("story_points");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.Property(x => x.Deadline)
                .HasColumnName("deadline")
                .HasColumnType("timestamp with time zone");

            builder.Property(x => x.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            builder.HasIndex(x => x.ProjectId)
                .HasDatabaseName("ix_task_items_project_id");

            builder.HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_task_items_projects_project_id");
        }
    }
}
