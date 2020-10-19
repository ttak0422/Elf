let 
  sources = import ./nix/sources.nix;
  pkgs = import sources.nixpkgs {};
in pkgs.mkShell {
  buildInputs = with pkgs; [
    nodejs
    yarn
    dotnet-sdk_3
    # keep this line if you use bash
    bashInteractive
  ];
}