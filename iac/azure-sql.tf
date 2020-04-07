resource "azurerm_sql_server" "asql" {
  name                         = join("", [var.projectname, "-asql"])
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = var.asql_administrator_login_name
  administrator_login_password = var.asql_administrator_login_password
}

variable "database_list" {
  default = {
    dev = "dev"
    qa  = "qa"
    prd = "prd"
  }
}

resource "azurerm_sql_database" "asqldb" {
  for_each = var.database_list

  name                = each.value
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  server_name         = azurerm_sql_server.asql.name
  edition             = "Basic"
}

resource "azurerm_sql_firewall_rule" "asqlfw-az-all" {
  name                = "AllowAllAzureIps"
  resource_group_name = azurerm_resource_group.rg.name
  server_name         = azurerm_sql_server.asql.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_sql_firewall_rule" "asqlfw-local-client" {
  name                = "Local"
  resource_group_name = azurerm_resource_group.rg.name
  server_name         = azurerm_sql_server.asql.name
  start_ip_address    = var.asql_local_client_ip_address
  end_ip_address      = var.asql_local_client_ip_address
}