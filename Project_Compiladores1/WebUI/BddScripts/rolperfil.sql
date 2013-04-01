delimiter $$

CREATE TABLE `rolperfil` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PerfilId` int(11) NOT NULL,
  `RolId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RolPerfil` (`PerfilId`,`RolId`),
  KEY `FK_RolId_idx` (`RolId`),
  KEY `FK_PerfilId_idx` (`PerfilId`),
  CONSTRAINT `FK_PerfilId` FOREIGN KEY (`PerfilId`) REFERENCES `perfil` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_Rol_X_Id` FOREIGN KEY (`RolId`) REFERENCES `rol` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci$$

