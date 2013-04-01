delimiter $$

CREATE TABLE `usuario` (
  `UsuarioId` int(11) NOT NULL AUTO_INCREMENT,
  `UsuarioNick` varchar(45) COLLATE latin1_spanish_ci NOT NULL,
  `UsuarioClave` varchar(500) COLLATE latin1_spanish_ci NOT NULL DEFAULT 'temporal',
  `UsuarioEstado` int(11) NOT NULL DEFAULT '0',
  `UsuarioPerfilId` int(11) NOT NULL,
  `UsuarioNombre` varchar(45) COLLATE latin1_spanish_ci NOT NULL,
  `UsuarioFoto` blob,
  `UsuarioEmail` varchar(50) COLLATE latin1_spanish_ci DEFAULT NULL,
  `UsuarioIsReset` int(11) DEFAULT '0',
  PRIMARY KEY (`UsuarioId`),
  KEY `fk_estadoUsuario_idx` (`UsuarioIsReset`),
  KEY `fk_estadoUsuario_idx1` (`UsuarioEstado`),
  KEY `fk_perfilUsuario_idx` (`UsuarioPerfilId`),
  CONSTRAINT `fk_estadoUsuario` FOREIGN KEY (`UsuarioEstado`) REFERENCES `estado` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_perfilUsuario` FOREIGN KEY (`UsuarioPerfilId`) REFERENCES `perfil` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci$$

