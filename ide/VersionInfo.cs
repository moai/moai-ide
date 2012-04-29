using Moai.Versioning;

#if __MONOCS__
[assembly: Mono()]
#else
[assembly: VisualStudio2008()]
#endif