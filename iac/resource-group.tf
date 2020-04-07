resource "azurerm_resource_group" "rg" {
  name     = join("", [var.project_name, "-rg"])
  location = var.location
}