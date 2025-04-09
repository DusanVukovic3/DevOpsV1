 variable "do_token" {
  type      = string
  sensitive = true
}

variable "ssh_fingerprint" {
  type        = string
  description = "Your SSH key fingerprint in DigitalOcean"
}

