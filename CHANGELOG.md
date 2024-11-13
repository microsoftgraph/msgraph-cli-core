# Changelog

## [1.3.1](https://github.com/microsoftgraph/msgraph-cli-core/compare/v1.3.0...v1.3.1) (2024-11-13)


### Bug Fixes

* removes reference to obsolete handler ([c79eb95](https://github.com/microsoftgraph/msgraph-cli-core/commit/c79eb9561fdec0ee7f27c63b1a0227c52a23293d))

## [1.3.0](https://github.com/microsoftgraph/msgraph-cli-core/compare/v1.2.2...v1.3.0) (2024-08-09)


### Features

* add web account manager (WAM) support ([#436](https://github.com/microsoftgraph/msgraph-cli-core/issues/436)) ([a5fe538](https://github.com/microsoftgraph/msgraph-cli-core/commit/a5fe538d448cb1a977942cfcf45bb1bbb57ac089))


### Bug Fixes

* add default scopes to solve login error when no scopes are provided ([a5fe538](https://github.com/microsoftgraph/msgraph-cli-core/commit/a5fe538d448cb1a977942cfcf45bb1bbb57ac089))

## [1.2.2](https://github.com/microsoftgraph/msgraph-cli-core/compare/v1.2.1...v1.2.2) (2024-07-25)


### Bug Fixes

* only fetch valid X.509 certificates from the certificate store ([#425](https://github.com/microsoftgraph/msgraph-cli-core/issues/425)) ([79db8c9](https://github.com/microsoftgraph/msgraph-cli-core/commit/79db8c90ea8a93d0f82847f2ea4e6dce3a1d8f52))

## [1.2.1](https://github.com/microsoftgraph/msgraph-cli-core/compare/v1.2.0...v1.2.1) (2024-02-13)


### Bug Fixes

* fix bug with authentication failure when environment is changed ([#347](https://github.com/microsoftgraph/msgraph-cli-core/issues/347)) ([eba1c70](https://github.com/microsoftgraph/msgraph-cli-core/commit/eba1c70f49ada4fafaf05a647fed580b60c6a6d1)), closes [#346](https://github.com/microsoftgraph/msgraph-cli-core/issues/346)

## [1.2.0](https://github.com/microsoftgraph/msgraph-cli-core/compare/v1.1.0...v1.2.0) (2024-02-05)


### Features

* add national cloud support ([#332](https://github.com/microsoftgraph/msgraph-cli-core/issues/332)) ([f78f591](https://github.com/microsoftgraph/msgraph-cli-core/commit/f78f5911ff9fa8a1dcf383431741face4f3c4fad))


### Performance Improvements

* enable concurrent io when clearing the token cache ([f78f591](https://github.com/microsoftgraph/msgraph-cli-core/commit/f78f5911ff9fa8a1dcf383431741face4f3c4fad))
