using QuestPDF.Fluent;
using QuestPDF.Previewer;
using QuestPDF.Infrastructure;
using Domain.Entities.UserMod;
using System;
using System.Collections.Generic;

// Resolve naming conflicts
using QColors = QuestPDF.Helpers.Colors;
using QContainer = QuestPDF.Infrastructure.IContainer;
using MainApp.ViewModels.UserModule;
using Domain.DTOs;
using MainApp.ViewModels.MenuModule;

public static class PdfExportService
{
    public static byte[] GenerateRolesPdf(List<Role> roles)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("Roles Export Report").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                        columns.ConstantColumn(130);
                        columns.ConstantColumn(80);
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Role Name").Bold();
                        header.Cell().Element(CellStyle).Text("System Role").Bold();
                        header.Cell().Element(CellStyle).Text("Permission Assigned").Bold();
                        header.Cell().Element(CellStyle).Text("Status").Bold();
                    });

                    // Table rows
                    foreach (var r in roles)
                    {
                        table.Cell().Element(CellStyle).Text(r.Name);
                        table.Cell().Element(CellStyle).Text(r.IsSystemRole ? "Yes" : "No");
                        table.Cell().Element(CellStyle).Text(r.IsPermissionAssigned ? "Yes" : "No");
                        table.Cell().Element(CellStyle).Text(r.IsActive ? "Active" : "Inactive");
                    }

                    QContainer CellStyle(QContainer container) =>
                        container.PaddingVertical(5).BorderBottom(1).BorderColor(QColors.Grey.Lighten2);
                });

                page.Footer().AlignCenter().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
            });
        });

        return doc.GeneratePdf();
    }

    public static byte[] GeneratePermissionGroupsPdf(List<PermissionGroup> roles)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("Permission Groups Export Report").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                        columns.ConstantColumn(130);
                        columns.ConstantColumn(80);
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Permission Group Name").Bold();
                        header.Cell().Element(CellStyle).Text("Code").Bold();
                        header.Cell().Element(CellStyle).Text("Model Associated").Bold();
                    });

                    // Table rows
                    foreach (var r in roles)
                    {
                        table.Cell().Element(CellStyle).Text(r.Name);
                        table.Cell().Element(CellStyle).Text(r.Code);
                        table.Cell().Element(CellStyle).Text(r.IsModelAssociated ? "Yes" : "No");
                    }

                    QContainer CellStyle(QContainer container) =>
                        container.PaddingVertical(5).BorderBottom(1).BorderColor(QColors.Grey.Lighten2);
                });

                page.Footer().AlignCenter().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
            });
        });

        return doc.GeneratePdf();
    }

    public static byte[] GeneratePermissionPdf(List<PermissionDTO> permissions)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("Permission Export Report").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                        columns.ConstantColumn(130);
                        columns.ConstantColumn(80);
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Permission Group Name").Bold();
                        header.Cell().Element(CellStyle).Text("Code").Bold();
                        header.Cell().Element(CellStyle).Text("Model Associated").Bold();
                    });

                    // Table rows
                    foreach (var r in permissions)
                    {
                        table.Cell().Element(CellStyle).Text(r.Groupname);
                        table.Cell().Element(CellStyle).Text(r.Action);
                        table.Cell().Element(CellStyle).Text(r.Description);
                        table.Cell().Element(CellStyle).Text(r.CreatedAt.ToString());
                    }

                    QContainer CellStyle(QContainer container) =>
                        container.PaddingVertical(5).BorderBottom(1).BorderColor(QColors.Grey.Lighten2);
                });

                page.Footer().AlignCenter().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
            });
        });

        return doc.GeneratePdf();
    }

    public static byte[] GenerateUsersPdf(List<UserViewModel> users)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("users Export Report").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                        columns.ConstantColumn(130);
                        columns.ConstantColumn(80);
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Name").Bold();
                        header.Cell().Element(CellStyle).Text("Email").Bold();
                        header.Cell().Element(CellStyle).Text("Phone").Bold();
                        header.Cell().Element(CellStyle).Text("Username").Bold();
                    });

                    // Table rows
                    foreach (var r in users)
                    {
                        table.Cell().Element(CellStyle).Text(r.Fullname);
                        table.Cell().Element(CellStyle).Text(r.Email);
                        table.Cell().Element(CellStyle).Text(r.PhoneNo);
                        table.Cell().Element(CellStyle).Text(r.Username);
                    }

                    QContainer CellStyle(QContainer container) =>
                        container.PaddingVertical(5).BorderBottom(1).BorderColor(QColors.Grey.Lighten2);
                });

                page.Footer().AlignCenter().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
            });
        });

        return doc.GeneratePdf();
    }

    public static byte[] GeneratePreparationAreasPdf(List<PreparationAreaViewModel> users)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("Preparation Area Export Report").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.ConstantColumn(100);
                        columns.ConstantColumn(130);
                        columns.ConstantColumn(80);
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Name").Bold();
                        header.Cell().Element(CellStyle).Text("Remarks").Bold();
                    });

                    // Table rows
                    foreach (var r in users)
                    {
                        table.Cell().Element(CellStyle).Text(r.Name);
                        table.Cell().Element(CellStyle).Text(r.Remarks);
                    }

                    QContainer CellStyle(QContainer container) =>
                        container.PaddingVertical(5).BorderBottom(1).BorderColor(QColors.Grey.Lighten2);
                });

                page.Footer().AlignCenter().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
            });
        });

        return doc.GeneratePdf();
    }

    public static byte[] GenerateCategoryPdf(List<MenuCategoryViewModel> users)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("Menu Category Export Report").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.ConstantColumn(130);
                        columns.ConstantColumn(100);
                        columns.ConstantColumn(80);
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Name").Bold();
                        header.Cell().Element(CellStyle).Text("Description").Bold();
                        header.Cell().Element(CellStyle).Text("Is Parent Category").Bold();
                        header.Cell().Element(CellStyle).Text("Display Order").Bold();
                    });

                    // Table rows
                    foreach (var r in users)
                    {
                        table.Cell().Element(CellStyle).Text(r.Name);
                        table.Cell().Element(CellStyle).Text(r.Description);
                        table.Cell().Element(CellStyle).Text(r.IsParentCategory ? "Yes" : "No");
                        table.Cell().Element(CellStyle).Text(r.DisplayOrder.ToString());
                    }

                    QContainer CellStyle(QContainer container) =>
                        container.PaddingVertical(5).BorderBottom(1).BorderColor(QColors.Grey.Lighten2);
                });

                page.Footer().AlignCenter().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
            });
        });

        return doc.GeneratePdf();
    }

    public static byte[] GenerateItemPdf(List<MenuItemViewModel> users)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("Menu Item Export Report").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.ConstantColumn(130);
                        columns.ConstantColumn(100);
                        columns.ConstantColumn(80);
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Name").Bold();
                        header.Cell().Element(CellStyle).Text("Description").Bold();
                        header.Cell().Element(CellStyle).Text("Is Parent Category").Bold();
                        header.Cell().Element(CellStyle).Text("Display Order").Bold();
                    });

                    // Table rows
                    foreach (var r in users)
                    {
                        table.Cell().Element(CellStyle).Text(r.Name);
                        table.Cell().Element(CellStyle).Text(r.Description);
                        table.Cell().Element(CellStyle).Text(r.IsActive ? "Yes" : "No");
                        table.Cell().Element(CellStyle).Text(r.DisplayOrder.ToString());
                    }

                    QContainer CellStyle(QContainer container) =>
                        container.PaddingVertical(5).BorderBottom(1).BorderColor(QColors.Grey.Lighten2);
                });

                page.Footer().AlignCenter().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
            });
        });

        return doc.GeneratePdf();
    }
}
