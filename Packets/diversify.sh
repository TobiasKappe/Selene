#!/bin/sh
for i in *.tar.gz; do
	base=$(basename $i .tar.gz)
	tar zxvf $i
	rm $i

	tar cjvf $base-$1.tar.bz2 $base/*
	tar czvf $base-$1.tar.gz $base/*
	zip $base-$1.zip $base/*
	rm -rf $base
done
