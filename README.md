# funccy
Functional data structures for C#

## Why?
Functional code helps you fall into the pit of success! When it's easier to do the right thing than 
the wrong thing, people tend to do it right.

## How?
### Immutable
### Maybe
A value that might be absent. Instead of returning a null, return a Maybe with no value.

```C#
// We have a value!
return new Maybe<MyType>(value);

// We don't have a value!
return new Maybe<MyType>();

// Use either one!
string x = result.Map(x => x.Foo).Extract("Nothing!");
```

### OneOf
A value that might be one of a few types. Good for error handling!

```C#
// We had no errors!
return new OneOf<Result, Error>(value);

// We had an error!
return new OneOf<Result, Error>(new Error("uh oh"));

// Use either one!
string x = result.Extract(x => x.Prop, e => e.Message);
```
