# Contributing Guide

I want to make contributing to this project as easy and transparent as
possible. Hopefully this document makes the process for contributing clear and
answers any questions you may have. If not, feel free to open an
[Issue](https://github.com/ratson/cordova-plugin-admob-free/issues).


## Issues

Please ensure your bug description is clear and has sufficient instructions
to be able to reproduce the issue. The best way is to provide a reduced
test case as a Github repository.


## Pull Requests

Your [pull requests](https://help.github.com/articles/creating-a-pull-request)
are always welcome and are greatly appreciated! Every little bit helps, and
credit will always be given.


### Getting Started

1. Fork this repo by using the "Fork" button in the upper-right

2. Check out your fork

   ```sh
   git clone git@github.com:yournamehere/cordova-plugin-admob-free.git

   cd cordova-plugin-admob-free
   ```

3. Install all dependencies

   ```sh
   npm install
   ```

4. Start coding!
   If you've added code, try add tests.
   If you've changed APIs, update any relevant documentation or tests.
   Ensure your work is committed within a feature branch.

5. Ensure all tests pass

   ```sh
   npm test
   ```


### Building Docs

Building the docs locally is extremely simple. First execute the following commands:

```sh
npm run docs
```

After this, you can open `gh-pages/index.html` in your browser.


### Keeping Fork Updated

Add git remote, call it `upstream`:

```sh
git remote add upstream https://github.com/ratson/cordova-plugin-admob-free.git
```

Each time you want to update, from your local `master` branch:

```sh
git fetch upstream
git rebase upstream/master
```

If you've rebased your branch onto `upstream/master` you may need to
force the push in order to push it to your own forked repository on GitHub.
You'd do that with:

```sh
git push -f origin master
```

For more reference, read [ESLint's excellent one](http://eslint.org/docs/developer-guide/contributing/pull-requests#working-with-code).


## For Collaborators

While collaborator is giving push access, it is better to send Pull Request, so otheres could review and comment.

Please note that generated code is not committed as it increases the repository size and need to rebuild frequently to be useful.

## License

By contributing to `cordova-plugin-admob-free`, you agree that
your contributions will be licensed under its MIT license.
