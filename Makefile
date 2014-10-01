.PHONE: default help xbuild msbuild

default: help

help:
	@echo "make msbuild|xbuild"

msbuild:
	@cd src && msbuild TransducersNet.sln

xbuild:
	@cd src && xbuild TransducersNet.sln

clean:
	-@rm -rf bin
	-@rm -rf src/TransducersNet/obj
	-@rm -rf src/TransducersNet.Tests/obj

realclean: clean
	-@rm -rf src/packages/*
