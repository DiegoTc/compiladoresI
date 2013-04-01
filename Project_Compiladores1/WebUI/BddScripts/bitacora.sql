delimiter $$

CREATE TABLE `bitacora` (
  `BitacoraId` int(11) NOT NULL AUTO_INCREMENT,
  `Bitacora_UsuarioId` int(11) NOT NULL,
  `BitacoraLenguaje` varchar(45) NOT NULL,
  `BitacoraDescripcionEvento` varchar(250) NOT NULL,
  `BitacoraSentencia` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`BitacoraId`),
  KEY `fk_bitacora_idx` (`Bitacora_UsuarioId`),
  CONSTRAINT `fk_bitacora` FOREIGN KEY (`Bitacora_UsuarioId`) REFERENCES `usuario` (`UsuarioId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8$$

