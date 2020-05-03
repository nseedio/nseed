# Versioning

We follow [Semantic Versioning](https://semver.org/).

The *Engine* and the *Tool* are treated as a single versionable unit:

- They have the same version.
- A breaking change in either of them is consider to be a breaking change of both.

The *Tool* is forward and backward compatible with the *Engine*.
In the case of a compatibility break (either forward or backward), the tool will display an error.
