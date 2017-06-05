[jQuery](http://jquery.com/) — New Wave JavaScript
==================================================

Contribution Guides
--------------------------------------

In the spirit of open source software development, jQuery always encourages community code contribution. To help you get started and before you jump into writing code, be sure to read these important contribution guidelines thoroughly:

## Including jQuery

Environments in which to use jQuery
--------------------------------------

- [Browser support](http://jquery.com/browser-support/) differs between the master branch and the 1.x branch. Specifically, the master branch does not support legacy browsers such as IE6-8. The jQuery team continues to provide support for legacy browsers on the 1.x branch. Use the latest 1.x release if support for those browsers is required. See [browser support](http://jquery.com/browser-support/) for more info.
- To use jQuery in Node, browser extensions, and other non-browser environments, use only master branch releases (2.x). The 1.x branch does not support these environments.

```html
<script src="https://code.jquery.com/jquery-2.2.0.min.js"></script>

```

### Rebasing ###

For feature/topic branches, you should always use the `--rebase` flag to `git pull`, or if you are usually handling many temporary "to be in a github pull request" branches, run the following to automate this:

```bash
git config branch.autosetuprebase local
```
(see `man git-config` for more information)

### Handling merge conflicts ###

If you're getting merge conflicts when merging, instead of editing the conflicted files manually, you can use the feature
`git mergetool`. Even though the default tool `xxdiff` looks awful/old, it's rather useful.

The following are some commands that can be used there:

* `Ctrl + Alt + M` - automerge as much as possible
* `b` - jump to next merge conflict
* `s` - change the order of the conflicted lines
* `u` - undo a merge
* `left mouse button` - mark a block to be the winner
* `middle mouse button` - mark a line to be the winner
* `Ctrl + S` - save
* `Ctrl + Q` - quit

[QUnit](http://api.qunitjs.com) Reference
-----------------

### Test methods ###

```js
expect( numAssertions );
stop();
start();
```


Note: QUnit's eventual addition of an argument to stop/start is ignored in this test suite so that start and stop can be passed as callbacks without worrying about their parameters

### Test assertions ###


```js
ok( value, [message] );
equal( actual, expected, [message] );
notEqual( actual, expected, [message] );
deepEqual( actual, expected, [message] );
notDeepEqual( actual, expected, [message] );
strictEqual( actual, expected, [message] );
notStrictEqual( actual, expected, [message] );
throws( block, [expected], [message] );
```


Test Suite Convenience Methods Reference (See [test/data/testinit.js](https://github.com/jquery/jquery/blob/master/test/data/testinit.js))
------------------------------

### Returns an array of elements with the given IDs ###

```js
q( ... );
```

Example:

```js
q("main", "foo", "bar");

=> [ div#main, span#foo, input#bar ]
```

### Asserts that a selection matches the given IDs ###

```js
t( testName, selector, [ "array", "of", "ids" ] );
```

Example:

```js
t("Check for something", "//[a]", ["foo", "bar"]);
```



### Fires a native DOM event without going through jQuery ###

```js
fireNative( node, eventType )
```

Example:

```js
fireNative( jQuery("#elem")[0], "click" );
```

### Add random number to url to stop caching ###

```js
url( "some/url.php" );
```

Example:

```js
url("data/test.html");

=> "data/test.html?10538358428943"


url("data/test.php?foo=bar");

=> "data/test.php?foo=bar&10538358345554"
```


### Load tests in an iframe ###

Loads a given page constructing a url with fileName: `"./data/" + fileName + ".html"`
and fires the given callback on jQuery ready (using the jQuery loading from that page)
and passes the iFrame's jQuery to the callback.

```js
testIframe( fileName, testName, callback );
```

Callback arguments:

```js
callback( jQueryFromIFrame, iFrameWindow, iFrameDocument );
```

### Load tests in an iframe (window.iframeCallback) ###

Loads a given page constructing a url with fileName: `"./data/" + fileName + ".html"`
The given callback is fired when window.iframeCallback is called by the page.
The arguments passed to the callback are the same as the
arguments passed to window.iframeCallback, whatever that may be.

```js
testIframeWithCallback( testName, fileName, callback );
```

Questions?
----------

If you have any questions, please feel free to ask on the
[Developing jQuery Core forum](http://forum.jquery.com/developing-jquery-core) or in #jquery on irc.freenode.net.
