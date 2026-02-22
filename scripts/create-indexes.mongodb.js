use enterprise_media_vault;

db.files.createIndex({ tenantId: 1, folderId: 1, name: 1 });
db.files.createIndex({ name: "text", mimeType: "text", type: "text" });
db.files.createIndex({ updatedAtUtc: -1 });
db.files.createIndex({ softDelete: 1 });

db.folders.createIndex({ tenantId: 1, parentFolderId: 1, name: 1 });
db.folders.createIndex({ softDelete: 1 });

db.auditLogs.createIndex({ createdAtUtc: -1 });
db.refreshTokens.createIndex({ expiresAtUtc: 1 }, { expireAfterSeconds: 86400 });

db.permissions.createIndex({ tenantId: 1, resourceId: 1, subjectId: 1, action: 1 });
