# giteabackup

一个简单的工具用于备份`gitea`并将备份文件上传到阿里云

## 使用说明

暂时未提供预编译安装包，需要自行编译部署。

```bash
# 创建程序运行时目录
mkdir /opt/giteabackup

# 目录结构
giteabackup
├── giteabackup/appsettings.json
├── giteabackup/giteabackup
└── giteabackup/giteabackup.pdb

# 设置systemd
cp deploy/giteabackup.service /etc/systemd/system/giteabackup.service

## 启动
systemctl start giteabackup
```

阿里云配置， 编辑`appsettings.json`

```json
"Aliyun": {
    "Endpoint": "阿里云端点地址",
    "AccessKeyId": "你的AccessKeyId",
    "AccessKeySecret": "你的AccessKeySecret",
    "BucketName": "你的BucketName"
  }
```

## 开发

### 依赖环境

- dotnet sdk 5.0
- Visual Studio or Visual Studio Code

### 编译

```bash
# 编译成单个执行文件
dotnet publish -r linux-x64 -p:PublishSingleFile=true --self-contained true
```

