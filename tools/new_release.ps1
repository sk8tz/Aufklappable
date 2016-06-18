$newTag = Read-Host 'Tag name (i.e. "v0.9"): '
cd ..
git tag $newTag
git push --tags
pause