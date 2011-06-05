ITUNES_HOME	= C:\Program Files (x86)\iTunes
ITUNES_COM_DLL	= "$(ITUNES_HOME)\iTunes.exe"
CC		= csc /nologo
LDFLAGS		= /l:iTunesLib.dll /t:winexe

all: iTunesC.exe

iTunesC.exe: iTunesC.cs iTunesLib.dll
	@$(CC) $(LDFLAGS) iTunesC.cs

iTunesLib.dll: $(ITUNES_COM_DLL)
	@tlbimp /nologo $(ITUNES_COM_DLL)

clean:
	@del iTunesLib.dll iTunesC.exe
