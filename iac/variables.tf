# values in terraform.tfvars
variable "location" {
  type        = string
  description = "Datacentre location for the application"
}

# dev values in dev.tfvars OR Azure Pipelines variables
variable "projectname" {
  type        = string
  description = "The project name (lower case characters only, no punctuation)"
}

variable "aks_client_id" {
  type        = string
  description = "The AKS service principal client ID"
}

variable "aks_client_secret" {
  type        = string
  description = "The AKS service principal client secret"
}

variable "asql_administrator_login_name" {
  type        = string
  description = "The Azure SQL administrator login name"
}
variable "asql_administrator_login_password" {
  type        = string
  description = "The Azure SQL administrator login password"
}

variable "asql_local_client_ip_address" {
  type        = string
  description = "The IP address for a local client"
}