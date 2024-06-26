#!/bin/bash

# 生成自签名证书和私钥
openssl req -x509 -newkey rsa:4096 -sha256 -days 3650 -nodes \
  -keyout localhost.key -out localhost.crt -subj "/CN=localhost" \
  -addext "subjectAltName=DNS:localhost,IP:127.0.0.1"

# 提示用户输入密码，如果直接按 Enter 则使用默认密码 "password"
read -sp "Enter a password for the .pfx file (or press Enter for default 'password'): " pfx_password
echo

# 如果用户直接按 Enter，则设置密码为默认密码
if [ -z "$pfx_password" ]; then
  pfx_password="password"
fi
# 将证书和私钥合并为 .pfx 文件
openssl pkcs12 -export -out localhost.pfx -inkey localhost.key -in localhost.crt -password pass:$pfx_password

cp localhost.pfx "../src/Rich.Webhook/"

cp localhost.crt "./Certificate/ca-certificates.crt"
mv localhost.pfx "./Certificate"
mv localhost.crt "./Certificate"
mv localhost.key "./Certificate"


#rm -rf ./localhost.crt
#rm -rf ./localhost.key

echo "Certificate and .pfx file have been created."
